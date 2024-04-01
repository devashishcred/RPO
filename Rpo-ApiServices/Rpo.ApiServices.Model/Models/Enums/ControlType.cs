// ***********************************************************************
// Assembly         : Rpo.ApiServices.Model
// Author           : Krunal Pandya
// Created          : 04-17-2018
//
// Last Modified By : Krunal Pandya
// Last Modified On : 04-17-2018
// ***********************************************************************
// <copyright file="ContrlType.cs" company="">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Rpo.ApiServices.Model.Models.Enums
{
    public enum ControlType
    {
        Text = 1,
        TextArea = 2,
        DropDown = 3,
        Radio = 4,
        Grid = 5,
        MultiselectDropdown = 6, // Listbox
        CheckBox = 7,
        DatePicker = 8,
        FileType = 9,
        TimePicker = 10,
        MultiSelectDatePicker = 11,
        Button = 12,
        MultipleDateTextBox = 13,
        TextAreaFor = 14,
    }
}
