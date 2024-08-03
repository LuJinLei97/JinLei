using System.Windows;

using JinLei.Extensions;

namespace JinLei.Classes;

public class RangeInfo
{
    public RangeInfo(int start = 0, int count = 0)
    {
        Start = start;
        Count = count;
    }

    public int Start { get; set; }

    public int Count { get; set; }

    public int AbsCount
    {
        get => Math.Abs(Count);
        set
        {
            if(value >= 0)
            {
                Count = value * Direction;
            }
        }
    }

    public int? Offset
    {
        get => IsEmpty ? null : (AbsCount - 1) * Direction;
        set
        {
            if(value.IsNull())
            {
                Count = 0;
            } else
            {
                Count = value.Value;
                AbsCount++;
            }
        }
    }

    public int? End { get => Start + Offset; set => Offset = value - Start; }

    public int Direction
    {
        get => Count >= 0 ? 1 : -1;
        set
        {
            if(value is 1 or -1 && value != Direction)
            {
                Reverse();
            }
        }
    }

    public bool IsEmpty => Count == 0;

    public int? Left => Direction == 1 ? Start : End;

    public int? Right => Direction == 1 ? End : Start;

    public void Reverse() => Count = -Count;

    public bool TryIntersect(RangeInfo rangeInfo, out RangeInfo result) => ((Rect)this).Do(t => t.Intersect(rangeInfo)).Do(t => t, out result).IsEmpty == false;

    public static implicit operator Rect(RangeInfo rangeInfo) => rangeInfo.IsEmpty ? Rect.Empty : new Rect(new System.Windows.Point(rangeInfo.Start, 0), new System.Windows.Point(rangeInfo.End.Value, 0));

    public static implicit operator RangeInfo(Rect rect) => rect.IsEmpty ? new RangeInfo() : new RangeInfo((int)rect.Left) { End = (int)rect.Right };
}