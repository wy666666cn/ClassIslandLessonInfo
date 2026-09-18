using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassIslandLessonInfo.Models;

/// <summary>
/// 「当前课程信息」组件的设置模型：可自定义字体大小、进度条与倒计时循环。
/// </summary>
public class CurrentLessonSettings : INotifyPropertyChanged
{
    /// <summary>科目 / 活动名的默认字号。</summary>
    public const int DefaultLessonFontSize = 20;
    /// <summary>时间范围的默认字号。</summary>
    public const int DefaultTimeFontSize = 14;
    /// <summary>进度条默认是否显示（默认关闭）。</summary>
    public const bool DefaultShowProgressBar = false;
    /// <summary>进度条默认粗度。</summary>
    public const int DefaultProgressBarThickness = 4;
    /// <summary>默认是否开启倒计时循环显示。</summary>
    public const bool DefaultEnableCountdownCycle = true;
    /// <summary>默认循环切换间隔（秒）：系统秒数是该间隔的倍数时翻转显示。</summary>
    public const int DefaultCycleInterval = 5;

    private int _lessonFontSize = DefaultLessonFontSize; // 科目 / 活动名 字号
    private int _timeFontSize = DefaultTimeFontSize;     // 时间范围 字号
    private bool _showProgressBar = DefaultShowProgressBar;  // 是否显示进度条
    private int _progressBarThickness = DefaultProgressBarThickness; // 进度条粗度
    private bool _enableCountdownCycle = DefaultEnableCountdownCycle; // 是否开启倒计时循环
    private int _cycleInterval = DefaultCycleInterval; // 循环切换间隔（秒）

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>科目 / 活动名称的字号。</summary>
    public int LessonFontSize
    {
        get => _lessonFontSize;
        set
        {
            if (_lessonFontSize == value) return;
            _lessonFontSize = value;
            OnPropertyChanged();
        }
    }

    /// <summary>时间范围的字号。</summary>
    public int TimeFontSize
    {
        get => _timeFontSize;
        set
        {
            if (_timeFontSize == value) return;
            _timeFontSize = value;
            OnPropertyChanged();
        }
    }

    /// <summary>是否在文字下方显示当前时间点的进度条。</summary>
    public bool ShowProgressBar
    {
        get => _showProgressBar;
        set
        {
            if (_showProgressBar == value) return;
            _showProgressBar = value;
            OnPropertyChanged();
        }
    }

    /// <summary>进度条的粗度（像素）。</summary>
    public int ProgressBarThickness
    {
        get => _progressBarThickness;
        set
        {
            if (_progressBarThickness == value) return;
            _progressBarThickness = value;
            OnPropertyChanged();
        }
    }

    /// <summary>是否开启倒计时循环显示（在科目与倒计时之间交替）。</summary>
    public bool EnableCountdownCycle
    {
        get => _enableCountdownCycle;
        set
        {
            if (_enableCountdownCycle == value) return;
            _enableCountdownCycle = value;
            OnPropertyChanged();
        }
    }

    /// <summary>循环切换间隔（秒）：系统秒数是该间隔的倍数时翻转显示（如 5 即 0/5/10/15…）。</summary>
    public int CycleInterval
    {
        get => _cycleInterval;
        set
        {
            if (_cycleInterval == value) return;
            _cycleInterval = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
