using System;
using System.Xml.Serialization;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 系统登录后存在Session中用户的信息
    /// </summary>
    [Serializable]
    public class SysLoginRight
    {
        private int _userId;
        [XmlElement]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        private int _empId;
        [XmlElement]
        public int EmpId
        {
            get { return _empId; }
            set { _empId = value; }
        }
        private string _empName;
        [XmlElement]
        public string EmpName
        {
            get { return _empName; }
            set { _empName = value; }
        }
        private int _deptId;
        [XmlElement]
        public int DeptId
        {
            get { return _deptId; }
            set { _deptId = value; }
        }
        private string _deptName;
        /// <summary>
        /// 当前登录科室
        /// </summary>
        [XmlElement]
        public string DeptName
        {
            get { return _deptName; }
            set { _deptName = value; }
        }
        private int _workId;
        [XmlElement]
        public int WorkId
        {
            get { return _workId; }
            set { _workId = value; }
        }

        private string _workName;
        [XmlElement]
        public string WorkName
        {
            get { return _workName; }
            set { _workName = value; }
        }

        [XmlElement]
        public Guid token { get; set; }
    }
}
