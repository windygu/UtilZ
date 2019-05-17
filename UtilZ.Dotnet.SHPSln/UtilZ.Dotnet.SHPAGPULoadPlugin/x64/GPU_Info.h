//*****************************************************************************
// 版本号：GPU_Info V20180423
// 版本说明：该动态库用于查询GPU的相关信息
// 注意：安装的显卡驱动版本不能低于该库使用的NVML库的版本
//*******************************************************************************

#pragma once

#ifdef __cplusplus
extern "C" {
#endif

//******************************************************************************
// 功能：查询GPU个数
// 输入：无
// 输出：n-GPU个数
// 返回值：true-调用成功；false-调用失败
__declspec(dllimport) bool GPUGetCount(unsigned int *n);

//******************************************************************************
// 功能：查询某个GPU内存信息
// 输入：devID-设备号
// 输出：totalMem-总的内存；freeMem-剩余内存；usedMem-已使用内存。单位均为byte。
// 返回值：true-调用成功；false-调用失败
__declspec(dllimport) bool GPUGetMemInfo(unsigned int devID, unsigned long long *totalMem, unsigned long long *freeMem, unsigned long long *usedMem);

//******************************************************************************
// 功能：查询某个GPU利用率
// 输入：devID-设备号
// 输出：utilizationRate-利用率（按百分比表示，如：utilizationRate=50，则利用率为50%）
// 返回值：true-调用成功；false-调用失败
__declspec(dllimport) bool GPUGetUtilizationRate(unsigned int devID, unsigned int *utilizationRate);

#ifdef __cplusplus
}
#endif