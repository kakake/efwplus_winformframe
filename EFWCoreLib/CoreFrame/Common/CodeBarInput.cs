using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// 条码枪扫码识别类
    /// </summary>
    public class CodeBarInput
    {
        public const int WM_SCANNER_INPUT = 0x0400 + 1345;      //条码扫描输入窗口消息
        public const int WM_RFID_INFO = 0x0400 + 1349;      //RFID读卡功能窗口消息
        public const int WM_IDCARD_INFO = 0x0400 + 1339;    //身份证读卡功能窗口消息
        
        public static bool CodeBarScanner = false;//开启扫描枪
        public static string RFIDReaderName = "";//精伦读卡器iDR210[长城]、SmartCardReader_KD8-4[长城]
        public static string IDCodeReaderName = "";//身份证读卡

        #region 扫描输入接口API声明
        /// <summary>
        /// 启用条码扫描输入
        /// </summary>
        /// <param name="hMainHwnd">接收扫描消息的消息句柄</param>
        /// <param name="lWndMsgNo">消息号</param>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "EnabledBarCodeScan", CharSet = CharSet.Ansi)]
        public static extern bool EnabledBarCodeScan(IntPtr hMainHwnd, int lWndMsgNo);

        //------------------------------------------------------------------------------------------------------------------------------------------------------    
        /// <summary>
        /// 禁用条码扫描输入
        /// </summary>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "DisabledBarCodeScan", CharSet = CharSet.Ansi)]
        public static extern void DisabledBarCodeScan();

        /// <summary>
        /// 启用RFID读卡扫描输入
        /// </summary>
        /// <param name="hMainHwnd">接收扫描消息的消息句柄</param>
        /// <param name="lWndMsgNo">消息号</param>
        /// <param name="pDevType">设备类型名称</param>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "EnabledRFIDScan", CharSet = CharSet.Ansi)]
        public static extern bool EnabledRFIDScan(IntPtr hMainHwnd, int lWndMsgNo, Byte[] cDevType);

        //------------------------------------------------------------------------------------------------------------------------------------------------------    
        /// <summary>
        /// 禁用RFID读卡扫描输入
        /// </summary>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "DisabledRFIDScan", CharSet = CharSet.Ansi)]
        public static extern void DisabledRFIDScan();

        /// <summary>
        /// 启用二代身份证读卡扫描输入
        /// </summary>
        /// <param name="hMainHwnd">接收扫描消息的消息句柄</param>
        /// <param name="lWndMsgNo">消息号</param>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "EnabledIDCardScan", CharSet = CharSet.Ansi)]
        public static extern bool EnabledIDCardScan(IntPtr hMainHwnd, int lWndMsgNo, Byte[] cDevType);

        //------------------------------------------------------------------------------------------------------------------------------------------------------    
        /// <summary>
        /// 禁用二代身份证读卡扫描输入
        /// </summary>
        /// <returns>0 成功</returns>
        [DllImport("CodeBar.dll", EntryPoint = "DisabledIDCardScan", CharSet = CharSet.Ansi)]
        public static extern void DisabledIDCardScan();
        #endregion

        #region 窗口消息函数
        [DllImport("user32.dll", EntryPoint = "PostMessageA", CharSet = CharSet.Ansi)]
        private static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessageA", CharSet = CharSet.Ansi)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible", CharSet = CharSet.Ansi)] 
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetParent", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetTopWindow", CharSet = CharSet.Ansi)]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);
        #endregion

        /// <summary>
        /// 启用所有配置好的扫描输入设备
        /// </summary>
        public static void EnabledAllCodeBarInput(IntPtr hMainHwnd)
        {
            if (CodeBarScanner)//
                EnabledBarCodeScan(hMainHwnd, WM_SCANNER_INPUT);
            String sDevName = RFIDReaderName;
            if (sDevName.Length > 0)
            {
                Byte[] sbMsg = System.Text.Encoding.Default.GetBytes(sDevName);
                EnabledRFIDScan(hMainHwnd, WM_RFID_INFO, sbMsg);
            }
            sDevName = IDCodeReaderName;
            if (sDevName.Length > 0)
            {
                Byte[] sbMsg = System.Text.Encoding.Default.GetBytes(sDevName);
                EnabledIDCardScan(hMainHwnd, WM_IDCARD_INFO, sbMsg);
            }
        }
       
        /// <summary>
        /// 禁止所有的扫描输入设备
        /// </summary>
        public static void DisabledAllCodeBarInput()
        {
            String sDevName = IDCodeReaderName;
            if (sDevName.Length > 0)
                DisabledIDCardScan();
            sDevName = RFIDReaderName;
            if (sDevName.Length > 0)
                DisabledRFIDScan();
            if (CodeBarScanner)
                DisabledBarCodeScan();
        }
    }
}