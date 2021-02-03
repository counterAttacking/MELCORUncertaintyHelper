﻿using MELCORUncertaintyHelper.Model;
using MELCORUncertaintyHelper.Service;
using MELCORUncertaintyHelper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MELCORUncertaintyHelper.Manager
{
    public class ExtractManager
    {
        private PTFFileOpenService ptfOpenSerivce;
        private InputVariableReadService inputReadService;
        private PTFFileReadService ptfReadService;
        private InputTimeReadService inputTimeReadService;
        private RefineDataProcessService refineProcessService;

        public ExtractManager()
        {

        }

        public async Task Run()
        {
            await Task.Run(() =>
            {
                this.ptfOpenSerivce = PTFFileOpenService.GetOpenService;
                var ptfFiles = (PTFFile[])this.ptfOpenSerivce.GetFiles();
                if (ptfFiles == null || ptfFiles.Length <= 0)
                {
                    MessageBox.Show("There is no PTF file", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.inputReadService = InputVariableReadService.GetInputReadService;
                var isInputManageFinished = this.inputReadService.InputManage();
                if (isInputManageFinished == false)
                {
                    return;
                }

                // 현재 Run()을 실행하면 데이터가 중복으로 생성되는 경우가 발생
                // 이를 임시적으로 해결하기 위한 방안
                ExtractDataManager.GetDataManager.InitializeData();

                //var frmStatus = StatusOutputForm.GetFrmStatus;

                for (var i = 0; i < ptfFiles.Length; i++)
                {
                    this.ptfReadService = new PTFFileReadService(ptfFiles[i]);
                    this.ptfReadService.Read();
                    var str = new StringBuilder();
                    /*str.Append("[");
                    str.Append(i + 1);
                    str.Append("] ");
                    str.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                    str.Append("Completed Read ");*/
                    str.AppendLine(ptfFiles[i].fullPath);
                    //frmStatus.PrintStatus(str);
                }
                /*Parallel.ForEach(ptfFiles, item =>
                {
                    this.ptfReadService = new PTFFileReadService(item);
                    this.ptfReadService.Read();
                    var str = new StringBuilder();
                    str.Append("[");
                    str.Append(i + 1);
                    str.Append("] ");
                    str.Append(DateTime.Now.ToString("[yyyy-MM-dd-HH:mm:ss]   "));
                    str.Append("Completed Read ");
                    str.AppendLine(item.fullPath);
                    frmStatus.PrintStatus(str);
                });*/

                this.inputTimeReadService = InputTimeReadService.GetInputTimeReadService;
                this.inputTimeReadService.ExtractTime();

                this.refineProcessService = new RefineDataProcessService();
                this.refineProcessService.Refine();
            });
        }
    }
}
