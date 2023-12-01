using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    /// <summary>
    /// 表格
    /// </summary>
    public class Table : ConsoleDraw
    {
        public Table(string title, int columns) : base(columns * 8)
        {
            Title= title;
            Columns= columns;
            _items = new();
            UnitWidth = 8;
            VerticalInterval = 1;
            DividingLineSymbol = '=';
            PrintAutoUnitWidth = true;
        }

        private readonly List<string[]> _items;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public readonly int Columns;

        /// <summary>
        /// 行数
        /// </summary>
        public int Rows => _items.Count;

        /// <summary>
        /// 单元宽度
        /// </summary>
        public int UnitWidth
        {
            get => _UnitWidth;
            set
            {
                if (value < 0)
                    throw new ArgumentException("单元宽度小于0");
                else _UnitWidth = value;
            }
        }
        private int _UnitWidth;

        /// <summary>
        /// 垂直间隔
        /// </summary>
        public int VerticalInterval
        {
            get => _VerticalInterval;
            set
            {
                if (value < 0)
                    throw new ArgumentException("垂直间隔小于0");
                else _VerticalInterval = value;
            }
        }
        private int _VerticalInterval;

        /// <summary>
        /// 分界线符号
        /// </summary>
        public char DividingLineSymbol
        {
            get => _DividingLineSymbol;
            set
            {
                if (ConsoleUtil.GetWidth(value) < 1)
                    throw new ArgumentException("分界线符号宽度小于0");
                else _DividingLineSymbol = value;
            }
        }
        private char _DividingLineSymbol;

        /// <summary>
        /// 是否在打印前自动调整单元格宽度
        /// </summary>
        public bool PrintAutoUnitWidth { get; set; }

        public void AddRow(string[] item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            if (item.Length != Columns)
                throw new ArgumentException("添加的行数据子项数量与列数限制不一致");

            _items.Add(item);
        }

        /// <summary>
        /// 自动计算单元宽度和总渲染区域宽度
        /// </summary>
        public void AutoUnitWidth()
        {
            List<string?> maxWidthList = new();

            foreach (var item in _items)
                maxWidthList.Add(ConsoleUtil.MaxWidthOf(item));
            UnitWidth = ConsoleUtil.GetWidth(ConsoleUtil.MaxWidthOf(maxWidthList)) + 2;
            Width = Columns * UnitWidth;
        }

        public override void Print()
        {
            if (Rows <= 0)
                return;

            if (PrintAutoUnitWidth)
                AutoUnitWidth();

            ApplyDrawColors();

            new DividingLine(Width, Title)
            {
                DividingLineSymbol = DividingLineSymbol
            }.Apply(this).Print();

            ApplyDrawColors();

            Line(VerticalInterval);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                    System.Console.Write(ConsoleUtil.GetToWidth(_items[i][j], UnitWidth));
                Line();

                Line(VerticalInterval);
            }

            ApplyInitialColors();
        }
    }
}
