using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilZ.Lib.Base.Threading
{
    /// <summary>
    /// 平行任务
    /// </summary>
    /// <typeparam name="T">参数类型</typeparam>
    public class BreezeParallelTask<T>: NThread
    {
        /// <summary>
        /// 参数队列集合
        /// </summary>
        private readonly Queue<T> _queues = null;

        /// <summary>
        /// 处理参数的委托
        /// </summary>
        private readonly Action<CancellationToken, T> ProcessAction;

        /// <summary>
        /// 线程上限数,-1为不限
        /// </summary>
        private readonly int _count = 0;

        /// <summary>
        /// 当前正在处理的任务集合
        /// </summary>
        private readonly List<Task> _tasks = new List<Task>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items">参数集合</param>
        /// <param name="action">处理参数的委托</param>
        /// <param name="count">线程上限数,-1为不限</param>
        internal BreezeParallelTask(IEnumerable<T> items, Action<CancellationToken, T> action, int count)
        {
            this._queues = new Queue<T>(items);
            this.ProcessAction = action;
            this._count = this._queues.Count <= count ? this._queues.Count : count;
        }

        /// <summary>
        /// 无参数的线程方法,CancellationToken对象每次执行时都是不同的变量
        /// </summary>
        /// <param name="token">线程取消通知参数</param>
        protected override void ExcuteThreadMethod(CancellationToken token)
        {
            ////如果线程取消
            if (token.IsCancellationRequested)
            {
                return;
            }

            while (true)
            {
                lock (this)
                {
                    //如果当前总共开启的任务数达到限制的线程数,则不再开启新的任务
                    if (this._count != -1 && this._tasks.Count == this._count)
                    {
                        break;
                    }

                    //开始一个平台处理任务
                    if (!this.CreateTask(token))
                    {
                        break;
                    }
                }
            }

            this.Wait();
        }

        /// <summary>
        /// 创建一个处理任务
        /// </summary>
        /// <returns>true:成功,false:失败</returns>
        private bool CreateTask(CancellationToken token)
        {
            if (this._queues.Count == 0)
            {
                return false;
            }

            //如果线程取消
            if (token.IsCancellationRequested)
            {
                return false;
            }

            var item = this._queues.Dequeue();
            Task task = new Task(new Action(() =>
            {
                this.ProcessAction(token, item);
            }));

            //当前任务执行完成后继续执行下一个任务
            task.ContinueWith((currentTask) =>
            {
                lock (this)
                {
                    this._tasks.Remove(currentTask);
                    this.CreateTask(token);
                }
            });

            this._tasks.Add(task);
            task.Start();
            return true;
        }

        /// <summary>
        /// 无限等待平行执行的线程结束
        /// </summary>
        private void Wait()
        {
            //等待线程执行结束
            while (true)
            {
                lock (this)
                {
                    if (this._tasks.Count == 0)
                    {
                        break;
                    }
                }

                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// 停止工作线程
        /// </summary>
        /// <param name="forcesCancellFlag">是否强制结束标示,true:将调用线程的Abort方法强制终止线程,false:通过线程取消通知来终止,但需要线程方法中对此作判断[默认为false]</param>
        /// <param name="throwOnfirtException">指示是否立即传播异常[默认为false]</param>
        public override void Stop(bool forcesCancellFlag = false, bool throwOnfirtException = false)
        {
            if (forcesCancellFlag)
            {
                //如果调用的是强制结束线程,则先调用基类的取消执行消息通知结束,再调用线程的Abort方法强制结束
                base.Stop(false, throwOnfirtException);
                base.Stop(true, throwOnfirtException);
            }
            else
            {
                base.Stop(forcesCancellFlag, throwOnfirtException);
            }
        }
    }
}
