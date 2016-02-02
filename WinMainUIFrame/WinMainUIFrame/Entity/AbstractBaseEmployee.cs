using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Orm;

namespace WinMainUIFrame.Entity
{
    [Serializable]
    [Table(TableName = "BaseEmployee", EntityType = EntityType.Table, IsGB = false)]
    public class BaseEmployee:EFWCoreLib.CoreFrame.Business.AbstractEntity
    {
        private int  _empid;
        /// <summary>
        /// 
        /// </summary>
        [Column(FieldName = "EmpId", DataKey = true,  Match = "", IsInsert = false)]
        public int EmpId
        {
            get { return  _empid; }
            set {  _empid = value; }
        }

        private string  _name;
        /// <summary>
        /// 姓名
        /// </summary>
        [Column(FieldName = "Name", DataKey = false,  Match = "", IsInsert = true)]
        public string Name
        {
            get { return  _name; }
            set {  _name = value; }
        }

        private int  _sex;
        /// <summary>
        /// 性别
        /// </summary>
        [Column(FieldName = "Sex", DataKey = false,  Match = "", IsInsert = true)]
        public int Sex
        {
            get { return  _sex; }
            set {  _sex = value; }
        }

        private DateTime  _brithday;
        /// <summary>
        /// 生日
        /// </summary>
        [Column(FieldName = "Brithday", DataKey = false,  Match = "", IsInsert = true)]
        public DateTime Brithday
        {
            get { return  _brithday; }
            set {  _brithday = value; }
        }

        private string  _szm;
        /// <summary>
        /// 数字码
        /// </summary>
        [Column(FieldName = "Szm", DataKey = false,  Match = "", IsInsert = true)]
        public string Szm
        {
            get { return  _szm; }
            set {  _szm = value; }
        }

        private string  _pym;
        /// <summary>
        /// 拼音码
        /// </summary>
        [Column(FieldName = "Pym", DataKey = false,  Match = "", IsInsert = true)]
        public string Pym
        {
            get { return  _pym; }
            set {  _pym = value; }
        }

        private string  _wbm;
        /// <summary>
        /// 五笔码
        /// </summary>
        [Column(FieldName = "Wbm", DataKey = false,  Match = "", IsInsert = true)]
        public string Wbm
        {
            get { return  _wbm; }
            set {  _wbm = value; }
        }

        private int  _delflag;
        /// <summary>
        /// 删除标记
        /// </summary>
        [Column(FieldName = "DelFlag", DataKey = false,  Match = "", IsInsert = true)]
        public int DelFlag
        {
            get { return  _delflag; }
            set {  _delflag = value; }
        }

    }
}
