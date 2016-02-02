using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DevComponents.DotNetBar;
using System.Text.RegularExpressions;


namespace EfwControls.CustomControl
{
    public partial class frmForm : Component
    {
        private bool _isSkip=true;
        public bool IsSkip
        {
            get { return _isSkip; }
            set { _isSkip = value; }
        }

        List<FormItem> itemlist;

        public frmForm()
        {
            itemlist = new List<FormItem>();
        }

        public frmForm(IContainer container)
            : this()
        {
            container.Add(this);

            InitializeComponent();
        }

        public void AddItem(Control _control, string _name)
        {
            AddItem(_control, _name, false, "", InvalidType.Empty, "", EN_CH.EN, null);
        }

        public void AddItem(Control _control, string _name, string _invalidMessage)
        {
            AddItem(_control, _name, true, _invalidMessage, InvalidType.Empty, "", EN_CH.EN, null);
        }

        public void AddItem(Control _control, string _name, string _invalidMessage, InvalidType _invalidType, string _invalidRules)
        {
            AddItem(_control, _name, true, _invalidMessage, _invalidType, _invalidRules, EN_CH.EN, null);
        }

        public void AddItem(Control _control, string _name, bool _required, string _invalidMessage, InvalidType _invalidType, string _invalidRules, EN_CH _inputMethod, object _defaultValue)
        {
            FormItem item = new FormItem();
            item.controlItem = _control;
            item.controlName = _name;
            item.required = _required;
            item.invalidMessage = _invalidMessage;
            item.invalidType = _invalidType;
            item.InvalidRules = _invalidRules;
            item.inputMethod = _inputMethod;
            item.defaultValue = _defaultValue;

            _control.Enter += new EventHandler(_control_Enter);
            _control.KeyUp += new KeyEventHandler(_control_KeyUp);
            itemlist.Add(item);
        }

        void _control_Enter(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;
            FormItem item = itemlist.Find(x => x.controlItem.Equals(ctl));
            int index = 0;//?
            InputLanguage currentLanguage = InputLanguage.InstalledInputLanguages[index];
            InputLanguage.CurrentInputLanguage = currentLanguage;
        }

        void _control_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)//回车调整
            {
                if (IsSkip == true)
                {
                    Control ctl = (Control)sender;
                    int index = itemlist.FindIndex(x => x.controlItem.Equals(ctl));
                    if (itemlist.Count - 1 > index)
                    {
                        itemlist[index + 1].controlItem.Focus();
                    }
                }
                else
                    IsSkip = true;
            }
        }

        private void SetValue(FormItem val, object value)
        {
            switch (val.controlItem.GetType().Name)
            {
                case "TextBoxX":
                    if (value == null)
                        val.controlItem.Text = "";
                    else
                        val.controlItem.Text = value.ToString();
                    break;
                case "ComboBoxEx":
                    if (value == null)
                        ((DevComponents.DotNetBar.Controls.ComboBoxEx)val.controlItem).Text = "";
                    else
                        ((DevComponents.DotNetBar.Controls.ComboBoxEx)val.controlItem).SelectedValue = value;
                    break;
                case "CheckBoxX":
                    if (value == null)
                        ((DevComponents.DotNetBar.Controls.CheckBoxX)val.controlItem).Checked = false;
                    else
                    {
                        ((DevComponents.DotNetBar.Controls.CheckBoxX)val.controlItem).CheckValue = value;
                        if (value.ToString() == "1") value = true;
                        else if (value.ToString() == "0") value = false;
                       
                        ((DevComponents.DotNetBar.Controls.CheckBoxX)val.controlItem).Checked = Convert.ToBoolean(value);
                    }
                    break;
                case "IntegerInput":
                    if (value == null)
                        ((DevComponents.Editors.IntegerInput)val.controlItem).Value = 0;
                    else
                        ((DevComponents.Editors.IntegerInput)val.controlItem).Value = Convert.ToInt32(value);
                    break;
                case "DoubleInput":
                    if (value == null)
                        ((DevComponents.Editors.DoubleInput)val.controlItem).Value = 0.00;
                    else
                        ((DevComponents.Editors.DoubleInput)val.controlItem).Value = Convert.ToDouble(value);
                    break;
                case "DateTimeInput":
                    if (value == null)
                        ((DevComponents.Editors.DateTimeAdv.DateTimeInput)val.controlItem).Value = DateTime.Now;
                    else
                        ((DevComponents.Editors.DateTimeAdv.DateTimeInput)val.controlItem).Value = Convert.ToDateTime(value);
                    break;

                case "TextBox":
                    if (value == null)
                        val.controlItem.Text = "";
                    else
                        val.controlItem.Text = value.ToString();
                    break;
                case "ComboBox":
                    if (value == null)
                        ((ComboBox)val.controlItem).Text = "";
                    else
                        ((ComboBox)val.controlItem).SelectedValue = value;
                    break;
                case "CheckBox":
                    if (value == null)
                        ((CheckBox)val.controlItem).Checked = false;
                    else
                    {
                        if (value.ToString() == "1") value = true;
                        else if (value.ToString() == "0") value = false;
                        ((CheckBox)val.controlItem).Checked = Convert.ToBoolean(value);
                    }
                    break;
                case "DateTimePicker":
                    if (value == null)
                        ((DateTimePicker)val.controlItem).Value = DateTime.Now;
                    else
                        ((DateTimePicker)val.controlItem).Value = Convert.ToDateTime(value);
                    break;

                case "NumericUpDown":
                    if (value == null)
                        ((NumericUpDown)val.controlItem).Value = 0;
                    else
                        ((NumericUpDown)val.controlItem).Value = Convert.ToDecimal(value);
                    break;
                case "RadioButton":
                    if (value == null)
                        ((RadioButton)val.controlItem).Checked = false;
                    else
                    {
                        //if (value.ToString() == "1") value = true;
                        //else if (value.ToString() == "0") value = false;
                        List<FormItem> _list = itemlist.FindAll(x => x.controlName == val.controlName);
                        if (_list.FindIndex(x => x.Equals(val)) == Convert.ToInt32(value))
                            ((RadioButton)val.controlItem).Checked = true;
                        else
                            ((RadioButton)val.controlItem).Checked = false;
                    }
                    break;
                case "TextBoxCard":
                    if (value == null)
                        ((TextBoxCard)val.controlItem).MemberValue = -1;
                    else
                        ((TextBoxCard)val.controlItem).MemberValue = value;
                    break;
                default:
                    if (value == null)
                        val.controlItem.Text = "";
                    else
                        val.controlItem.Text = value.ToString();
                    break;
            }
        }
        private object GetValue(FormItem val)
        {
            switch (val.controlItem.GetType().Name)
            {
                case "TextBoxX":
                    return val.controlItem.Text;

                case "ComboBoxEx":
                    return ((DevComponents.DotNetBar.Controls.ComboBoxEx)val.controlItem).SelectedValue;

                case "CheckBoxX":
                    object ckval = ((DevComponents.DotNetBar.Controls.CheckBoxX)val.controlItem).CheckValue;
                    if (ckval.ToString() == "Y")
                        ckval = "1";
                    else if (ckval.ToString() == "N")
                        ckval = "0";
                    return ckval;

                case "IntegerInput":
                    return ((DevComponents.Editors.IntegerInput)val.controlItem).Value;

                case "DoubleInput":
                    return ((DevComponents.Editors.DoubleInput)val.controlItem).Value;

                case "DateTimeInput":
                    return ((DevComponents.Editors.DateTimeAdv.DateTimeInput)val.controlItem).Value;


                case "TextBox":
                    return val.controlItem.Text;

                case "ComboBox":
                    return ((ComboBox)val.controlItem).SelectedValue;

                case "CheckBox":
                    return ((CheckBox)val.controlItem).Checked;

                case "DateTimePicker":
                    return ((DateTimePicker)val.controlItem).Value;


                case "NumericUpDown":
                    return ((NumericUpDown)val.controlItem).Value;

                case "RadioButton":
                    return ((RadioButton)val.controlItem).Checked;

                case "TextBoxCard":
                    return ((TextBoxCard)val.controlItem).MemberValue;

                default:
                    return val.controlItem.Text;

            }
        }

        public void Clear()
        {
            foreach (FormItem val in itemlist)
            {
                SetValue(val, val.defaultValue);
            }

            if (itemlist.Count > 0) itemlist[0].controlItem.Focus();
        }

        public void Load<T>(T data)
        {
            foreach (FormItem val in itemlist)
            {
                System.Reflection.PropertyInfo pro = data.GetType().GetProperty(val.controlName);
                if (pro != null)
                {
                    object value = pro.GetValue(data, null);
                    SetValue(val, value);
                }
            }
        }

        public void Load(Dictionary<string, object> data)
        {
            foreach (FormItem val in itemlist)
            {

                object value = data[val.controlName];
                SetValue(val, value);

            }
        }

        public void Load(DataRow data)
        {
            foreach (FormItem val in itemlist)
            {
                object value = data[val.controlName];
                SetValue(val, value);
            }
        }

        public void GetValue<T>(T t)
        {
            foreach (FormItem val in itemlist)
            {
                System.Reflection.PropertyInfo pro = t.GetType().GetProperty(val.controlName);
                if (pro != null)
                {
                    object itemval = GetValue(val);
                    if (itemval == null) continue;
                    if (val.controlItem.GetType().Name == "RadioButton")
                    {
                        if (Convert.ToBoolean(itemval) == true)
                        {
                            List<FormItem> _list = itemlist.FindAll(x => x.controlName == val.controlName);
                            int _index = _list.FindIndex(x => x.Equals(val));

                            pro.SetValue(t, _index, null);
                        }
                    }
                    else
                    {
                        if (pro.PropertyType == typeof(System.Int32) && itemval.GetType() == typeof(System.Boolean))
                            itemval = Convert.ToBoolean(itemval) ? 1 : 0;
                        else if (pro.PropertyType == typeof(System.Int32))
                            itemval = Convert.ToInt32(itemval);
                        else if (pro.PropertyType == typeof(System.String))
                            itemval = itemval == null ? "" : itemval.ToString();
                        else if (pro.PropertyType == typeof(System.DateTime))
                            itemval = Convert.ToDateTime(itemval);
                        else if (pro.PropertyType == typeof(System.Decimal))
                            itemval = Convert.ToDecimal(itemval);
                        pro.SetValue(t, itemval, null);
                    }
                }
            }
        }

        public Dictionary<string, object> GetValue()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (FormItem val in itemlist)
            {
                dic.Add(val.controlName, GetValue(val));
            }

            return dic;
        }

        public bool Validate()
        {
            foreach (FormItem val in itemlist)
            {
                if (val.required)
                {
                    bool b = true;

                    switch (val.controlItem.GetType().Name)
                    {
                        case "TextBoxX":
                        case "ComboBoxEx":
                        case "IntegerInput":
                        case "DoubleInput":
                        case "DateTimeInput":
                        case "TextBox":
                        case "ComboBox":
                        case "DateTimePicker":
                        case "NumericUpDown":
                        case "TextBoxCard":
                            b = ValidateType(val.invalidType, val.InvalidRules, val.controlItem.Text);
                            break;

                    }

                    if (b == false)
                    {
                        MessageBoxEx.Show(val.invalidMessage, "验证提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        val.controlItem.Focus();
                        return false;
                    }
                }
            }

            return true;
        }

        public bool ValidateType(InvalidType _type, string _rules, string _text)
        {
            Regex regex;
            string rules;
            switch (_type)
            {
                case InvalidType.Empty:
                    if (_text.Trim() == "")
                        return false;
                    break;
                case InvalidType.Phone:
                    if (_text.Trim() != "")
                    {
                        rules = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";//电话号码 或 手机号码
                        regex = new Regex(rules);
                        return regex.IsMatch(_text.Trim());
                    }
                    break;
                case InvalidType.IDcard:
                    if (_text.Trim() != "")
                    {
                        rules = @"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$";//身份证
                        regex = new Regex(rules);
                        return regex.IsMatch(_text.Trim());
                    }
                    break;
                case InvalidType.Custom:
                    if (_text.Trim() != "")
                    {
                        rules = _rules;
                        regex = new Regex(rules);
                        return regex.IsMatch(_text.Trim());
                    }
                    break;
            }
            return true;
        }

        public void SetEnabled(bool enabled)
        {
            foreach (FormItem val in itemlist)
            {
                val.controlItem.Enabled = enabled;
            }
        }
    }

    public class FormItem
    {
        public string controlName { get; set; }
        public object controlValue { get; set; }
        public object defaultValue { get; set; }

        public Control controlItem { get; set; }
        public bool required { get; set; }
        public string invalidMessage { get; set; }
        public InvalidType invalidType { get; set; }
        public string InvalidRules { get; set; }

        public EN_CH inputMethod { get; set; }
    }

    /// <summary>
    /// 中英文输入法自动切换
    /// </summary>
    public enum EN_CH
    {
        EN, CH
    }

    public enum InvalidType{
        Empty,Phone, IDcard,Custom
    }
}
