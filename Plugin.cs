using ClassIsland.Core;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Extensions.Registry;
using ClassIslandLessonInfo.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassIslandLessonInfo;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        // 注册“当前课程信息”组件及其设置界面，使其出现在【应用设置】→【组件】的组件库中
        services.AddComponent<CurrentLessonComponent, CurrentLessonSettingsControl>();
    }
}
