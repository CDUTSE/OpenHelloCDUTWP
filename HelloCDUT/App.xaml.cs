using DataHelper.Helper;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using 你好理工.DataHelper.Model;
using 你好理工.View;
using 你好理工.View.Auth;

// “空白应用程序”模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace 你好理工
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        
        
        #region 后台任务
        //后台任务名称
        private const string TASK_NAME = "TileBackground";
        //入口点
        private const string ENTRY_POINT = "TileBackgroundTask.TileBackground";
        #endregion
        /// <summary>
        /// 初始化单一实例应用程序对象。    这是执行的创作代码的第一行，
        /// 逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            RequestedTheme = ApplicationTheme.Light;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            RegisterBackgroundTask();

            // https://github.com/kiwidev/WinRTExceptions
            this.UnhandledException += OnUnhandledException;
        }

        /// <summary>
        /// 处理同步异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog("Application Unhandled Exception:\r\n" + GetExceptionDetailMessage(e.Exception), "(╯‵□′)╯︵┻━┻")
                .ShowAsync();
        }

        /// <summary>
        /// 处理异步异常
        /// Should be called from OnActivated and OnLaunched
        /// </summary>
        private void RegisterExceptionHandlingSynchronizationContext()
        {
            ExceptionHandlingSynchronizationContext
                .Register()
                .UnhandledException += SynchronizationContext_UnhandledException;
        }

        private async void SynchronizationContext_UnhandledException(object sender, global::DataHelper.Helper.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            await new MessageDialog("Synchronization Context Unhandled Exception:\r\n" + GetExceptionDetailMessage(e.Exception), "(╯‵□′)╯︵┻━┻")
                .ShowAsync();
        }

        // https://github.com/ljw1004/async-exception-stacktrace
        //获取简洁的错误信息
        private string GetExceptionDetailMessage(Exception ex)
        {
            return ex.Message + Environment.NewLine + ex.StackTraceEx();
        }

        #region 全局保存的用户信息
        /// <summary>
        /// 学期开始的月份
        /// </summary>
        public int term_beginMonth { get; set; }
        /// <summary>
        /// 学期开始的日期
        /// </summary>
        public int term_beginDay { get; set; }
        /// <summary>
        /// 全局保存的user_login_token
        /// </summary>
        public string user_login_token { get; set; }

        /// <summary>
        /// 最大周数
        /// </summary>
        public int max_weeknum { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string user_name { get; set; }
        public string user_aao_status { get; set; }
        public string user_campus_status { get; set; }
        public string user_lib_status { get; set; }
        public string user_email_status { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string user_nick_name { get; set; }
        public string user_password_hash { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string user_stu_id { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string user_email { get; set; }
        
        public string user_chat_token { get; set; }
        public string user_gender { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string user_motto { get; set; }
        /// <summary>
        /// 恋爱状态
        /// </summary>
        public string user_love_status { get; set; }
        /// <summary>
        /// 性取向
        /// </summary>
        public string user_sex_orientation { get; set; }
        /// <summary>
        /// 认证实名
        /// </summary>
        public string user_real_name { get; set; }
        public string user_birthdate { get; set; }
        /// <summary>
        /// 学院
        /// </summary>
        public string user_institute { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string user_major { get; set; }
        /// <summary>
        /// 班号
        /// </summary>
        public string user_class_id { get; set; }
        /// <summary>
        /// 入学年份
        /// </summary>
        public string user_entrance_year { get; set; }
        /// <summary>
        /// 头像url
        /// </summary>
        public string user_avatar_url { get; set;}
        public string user_love_status_permission { get; set; }
        public string user_sex_orientation_permission { get; set; }
        public string user_real_name_permission { get; set; }
        public string user_birthdate_permission { get; set; }
        public string user_stu_num_permission { get; set; }
        public string user_institute_id_permission { get; set; }
        public string user_major_permission { get; set; }
        public string user_class_id_permission { get; set; }
        public string user_entrance_year_permission { get; set; }
        public string user_schedule_permission { get; set; }
        public string user_group_schedule_permission { get; set; }
        #endregion

        /// <summary>
        /// 注册后台任务
        /// </summary>
        private async void RegisterBackgroundTask()
        {
            //检查是否允许后台任务
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity)
            {
                //检查是否已经注册后台任务
                var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault((t) => t.Name == TASK_NAME);
                //如果未注册，则进行注册
                if (task == null)
                {
                    BackgroundTaskBuilder backgroundTaskBuild = new BackgroundTaskBuilder();
                    backgroundTaskBuild.TaskEntryPoint = ENTRY_POINT;
                    backgroundTaskBuild.Name = TASK_NAME;
                    //触发器为时间
                    backgroundTaskBuild.SetTrigger(new TimeTrigger(20, false));

                    backgroundTaskBuild.Register();
                }
             
            }
        }

        /// <summary>
        /// 处理后台任务完成的事件处理
        /// </summary>
        /// <param name="task"></param>
        /// <param name="args"></param>
        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            try
            {
                args.CheckResult();

            }
            catch (Exception e)
            {
                Debug.WriteLine("OnCompleted : "+e.Message);
            }
        }

        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            Debug.WriteLine("Progress : " + args.Progress + "%");
        }

         
        /// <summary>
        /// 注销后台任务
        /// </summary>
        private void UnRegister()
        {
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(t => t.Name == TASK_NAME);
            if (task != null)
            {
                task.Unregister(true);
                Debug.WriteLine("后台任务已注销");
            }
        }
     
        /// <summary>
        /// 后退处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame.SourcePageType ==typeof(Login)) return;
            if(frame == null)
            {
                return;
            }
            if(frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
            else
            {
                App.Current.Exit();
            }
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以打开特定的文件或显示搜索结果等操作时，
        /// 将使用其他入口点。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                // TODO: 将此值更改为适合您的应用程序的缓存大小
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 删除用于启动的旋转门导航。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 新页面
                #if DEBUG
                    Debug.WriteLine("Settings.IsFirstLaunch："+Settings.Instance.IsFirstLaunch);
                #endif

                    //if (e.PreviousExecutionState != ApplicationExecutionState.Running)
                    //{
                    //    bool loadState = (e.PreviousExecutionState == ApplicationExecutionState.Terminated);
                    //    SplashScreenPage extendedSplash = new SplashScreenPage(e.SplashScreen, loadState);
                    //    Window.Current.Content = extendedSplash;
                    //}

                    if (Settings.Instance.IsFirstLaunch)  //第一次启动
                    {
                        if (!rootFrame.Navigate(typeof(WelComePage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                    else
                    {
                        if (!rootFrame.Navigate(typeof(Login), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
            }

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 启动应用程序后还原内容转换。
        /// </summary>
        /// <param name="sender">附加了处理程序的对象。</param>
        /// <param name="e">有关导航事件的详细信息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。    在不知道应用程序
        /// 将被终止还是恢复的情况下保存应用程序状态，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起的请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }

        
    }
}