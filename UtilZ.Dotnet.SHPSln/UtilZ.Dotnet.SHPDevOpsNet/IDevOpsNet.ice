#pragma once
module UtilZ
{
	module Dotnet
	{
		module SHPDevOpsNet
		{
			interface IDevOpsControl
			{
				/**
				* 下达命令
				**/
				["amd"] string SendCommand(string cmdStr);
			
			};
		};
	};
};