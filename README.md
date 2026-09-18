# ClassIslandLessonInfo

一个 ClassIsland 组件插件：在主界面显示**当前课程、上下课时间、距下课/上课倒计时、下一节课**。

## 功能

在 ClassIsland 主界面添加名为「当前课程信息」的组件后，会实时显示：

- 当前课程科目（课间时显示课间名称）
- 当前时间点起止时间（如 `08:00 - 08:45`）
- 上课中：距下课倒计时（`距下课 12:34`）
- 课间：距上课倒计时（`距上课 03:05`）
- 下一节课科目及其开始时间

数据来源为 ClassIsland 的 `ILessonsService`（插件内通过依赖注入获取，不依赖跨进程 IPC）。

## 安装（复制粘贴即用）

1. 将 `ClassIslandLessonInfo` 文件夹整体复制到 ClassIsland 的插件目录：
   ```
   <ClassIsland安装目录>\data\Plugins\
   ```
   最终路径形如：`...\data\Plugins\ClassIslandLessonInfo\`（内含 `ClassIslandLessonInfo.dll`、`manifest.yml`、`icon.png`）。
2. **重启 ClassIsland**（托盘图标右键 → 退出，再重新打开）。
3. 打开【应用设置】→【组件】，在组件库中找到「当前课程信息」，拖到主界面即可。

卸载：直接删除 `data\Plugins\ClassIslandLessonInfo` 文件夹。

## 开发 / 构建

环境：.NET 8 SDK。

```powershell
cd ClassIslandLessonInfo
dotnet build -c Release
dotnet publish -c Release -o publish   # publish 目录即为可分发的插件文件夹
```

## 项目结构

```
ClassIslandLessonInfo/
├─ ClassIslandLessonInfo.csproj     # 引用 ClassIsland.PluginSdk
├─ Plugin.cs                        # 插件入口，注册组件
├─ manifest.yml                     # 插件清单（入口程序集、apiVersion 等）
├─ Components/
│  ├─ CurrentLessonComponent.axaml     # 组件界面
│  └─ CurrentLessonComponent.axaml.cs  # 组件逻辑，注入 ILessonsService
└─ icon.png
```

## 关键接口

| 成员 | 含义 |
|---|---|
| `ILessonsService.CurrentSubject` | 当前科目 |
| `ILessonsService.CurrentTimeLayoutItem` | 当前时间点，含 `StartTime`/`EndTime` |
| `ILessonsService.OnBreakingTimeLeftTime` | 距下课剩余时间 |
| `ILessonsService.OnClassLeftTime` | 距上课剩余时间 |
| `ILessonsService.NextClassSubject` / `NextClassTimeLayoutItem` | 下一节课科目及时间点 |
| `ILessonsService.CurrentState` | 当前状态（OnClass / Breaking / AfterSchool） |

## 参考

- ClassIsland 开发文档：<https://docs.classisland.tech/dev/>
- 组件文档：<https://docs.classisland.tech/dev/components.html>
