using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared.Enums;
using ClassIsland.Shared.Models.Profile;
using ClassIslandLessonInfo.Models;

namespace ClassIslandLessonInfo.Components;

/// <summary>
/// 当前课程信息组件：单行显示「当前科目 + 时间范围」，样式对齐官方课表；
/// 文字下方可开启当前时间点进度条（默认关闭，粗度可调）；
/// 可开启倒计时循环显示：主文字在「科目名」与「倒计时」之间按指定秒数点交替。
/// </summary>
[ComponentInfo(
    "57DB00F5-C286-4475-999D-956553D0F634", // 组件唯一 GUID
    "当前课程信息",
    "\uE9B0",
    "单行显示当前课程名称与上下课时间范围，可开启进度条与倒计时循环显示。")]
public partial class CurrentLessonComponent : ComponentBase<CurrentLessonSettings>
{
    private readonly ILessonsService _lessons;
    private readonly DispatcherTimer _timer;
    // 切换模糊动画计时器
    private DispatcherTimer? _blurTimer;

    // 倒计时循环显示状态：当前是否显示倒计时（否则显示科目名）
    private bool _showCountdown;
    // 上一次触发切换的秒数，避免同一秒重复翻转
    private int _lastSwitchSecond = -1;

    public CurrentLessonComponent(ILessonsService lessonsService)
    {
        _lessons = lessonsService;
        InitializeComponent();

        // 每秒刷新一次，以便课节切换、进度条与循环显示及时更新
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => Refresh();

        AttachedToVisualTree += (_, _) =>
        {
            _timer.Start();
            Refresh();
        };
        DetachedFromVisualTree += (_, _) => _timer.Stop();
    }

    /// <summary>刷新显示：按当前活动状态显示名称 + 时间范围 + 进度条 + 循环切换。</summary>
    private void Refresh()
    {
        try
        {
            var state = _lessons.CurrentState;
            var timeLayout = _lessons.CurrentTimeLayoutItem;

            // 倒计时循环显示：系统秒数是循环间隔的倍数时（间隔5即 0/5/10/15…）翻转「科目名 / 倒计时」
            if (Settings is not null && Settings.EnableCountdownCycle)
            {
                var nowSecond = DateTime.Now.Second;
                var interval = Settings.CycleInterval;
                if (interval > 0 && nowSecond % interval == 0 && nowSecond != _lastSwitchSecond)
                {
                    _showCountdown = !_showCountdown;
                    _lastSwitchSecond = nowSecond;
                    AnimateBlurIn(); // 切换时模糊过渡
                }
            }

            // 主文字显示：开启循环且当前轮到倒计时 -> 显示倒计时；否则按状态显示活动名称
            if (Settings is not null && Settings.EnableCountdownCycle && _showCountdown)
                LessonText.Text = GetCountdownText(state);
            else
                LessonText.Text = state switch
                {
                    TimeState.OnClass => _lessons.CurrentSubject?.Name ?? "上课",
                    TimeState.Breaking => _lessons.CurrentSubject?.Name ?? "课间",
                    TimeState.AfterSchool => "放学",
                    _ => _lessons.CurrentSubject?.Name ?? "未上课"
                };

            // 只有存在有效时间点时才显示时间范围，否则清空（避免 00:00-00:00）
            if (timeLayout.StartTime != TimeSpan.Zero || timeLayout.EndTime != TimeSpan.Zero)
                TimeText.Text = $"{Fmt(timeLayout.StartTime)}-{Fmt(timeLayout.EndTime)}";
            else
                TimeText.Text = string.Empty;

            UpdateProgress(timeLayout);
        }
        catch
        {
            // 读取失败时保持原显示，避免组件崩溃
        }
    }

    /// <summary>倒计时文字：按当前时间点状态加前缀——上课=距下课，课间/准备上课=距上课，放学=已放学。</summary>
    private string GetCountdownText(TimeState state) => state switch
    {
        TimeState.OnClass => $"距下课 {_lessons.OnBreakingTimeLeftTime:mm\\:ss}",
        TimeState.Breaking or TimeState.PrepareOnClass => $"距上课 {_lessons.OnClassLeftTime:mm\\:ss}",
        TimeState.AfterSchool => "已放学",
        _ => "已放学"
    };

    /// <summary>切换时的模糊过渡：先把文字模糊，再逐帧收清晰。</summary>
    private void AnimateBlurIn()
    {
        _blurTimer?.Stop();
        if (LessonText.Effect is not BlurEffect blur) return;
        blur.Radius = 10;
        _blurTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(15) };
        _blurTimer.Tick += (_, _) =>
        {
            blur.Radius -= 1.4;
            if (blur.Radius <= 0)
            {
                blur.Radius = 0;
                _blurTimer?.Stop();
            }
        };
        _blurTimer.Start();
    }

    /// <summary>更新进度条：当前时间在当前时间点内的进度（0~1）。</summary>
    private void UpdateProgress(TimeLayoutItem timeLayout)
    {
        // Settings 在组件初始化完成后才可用；关闭时进度置 0
        if (Settings is null || !Settings.ShowProgressBar)
        {
            ProgressBar.Value = 0;
            return;
        }

        var duration = timeLayout.EndTime - timeLayout.StartTime;
        if (duration <= TimeSpan.Zero)
        {
            ProgressBar.Value = 0;
            return;
        }

        var now = DateTime.Now.TimeOfDay;
        var elapsed = now - timeLayout.StartTime;
        ProgressBar.Value = Math.Clamp(elapsed.TotalMilliseconds / duration.TotalMilliseconds, 0, 1);
    }

    private static string Fmt(TimeSpan ts) => $"{(int)ts.TotalHours:00}:{ts.Minutes:00}";
}
