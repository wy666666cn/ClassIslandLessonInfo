using Avalonia.Interactivity;
using ClassIsland.Core.Abstractions.Controls;
using ClassIslandLessonInfo.Models;

namespace ClassIslandLessonInfo.Components;

/// <summary>
/// 「当前课程信息」组件的设置界面，用于自定义各部分的字体大小。
/// </summary>
public partial class CurrentLessonSettingsControl : ComponentBase<CurrentLessonSettings>
{
    public CurrentLessonSettingsControl()
    {
        InitializeComponent();
    }

    /// <summary>将各项设置恢复为默认值。</summary>
    private void ResetDefaults_Click(object? sender, RoutedEventArgs e)
    {
        Settings.LessonFontSize = CurrentLessonSettings.DefaultLessonFontSize;
        Settings.TimeFontSize = CurrentLessonSettings.DefaultTimeFontSize;
        Settings.ShowProgressBar = CurrentLessonSettings.DefaultShowProgressBar;
        Settings.ProgressBarThickness = CurrentLessonSettings.DefaultProgressBarThickness;
        Settings.EnableCountdownCycle = CurrentLessonSettings.DefaultEnableCountdownCycle;
        Settings.CycleInterval = CurrentLessonSettings.DefaultCycleInterval;
    }
}
