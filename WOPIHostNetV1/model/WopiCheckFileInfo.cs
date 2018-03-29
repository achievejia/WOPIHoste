using System.Runtime.Serialization;

namespace WOPIHostNetV1.model
{
    [DataContract]
    public class WopiCheckFileInfo
    {
        /// <summary>
        /// 表示WOPI客户端允许连接文件中对于外部服务的引用（例如一个可以嵌入JavaSCript应用的市场），如果这个值是false，那么这个客户端则不允许建立这样的连接。
        /// </summary>
        [DataMember]
        public bool AllowExternalMarketplace { get; set; }

        /// <summary>
        /// 不包括路径的文件名，用来在界面(UI)中展示，同时也用来确定这个文件的拓展名。
        /// </summary>
        [DataMember]
        public string BaseFileName { get; set; }

        /// <summary>
        /// 表示WOPI客户端向用户展示WOPI服务器的品牌。
        /// </summary>
        [DataMember]
        public string BreadcrumbBrandName { get; set; }

        /// <summary>
        ///  一个指向网页的统一资源表示符(URI)，当用户点击BreadcrumbBrandName展示的内容时会跳转到URI指向的地址。 
        /// </summary>
        [DataMember]
        public string BreadcrumbBrandUrl { get; set; }

        /// <summary>
        /// 表示WOPI客户端向用户展示用于表示文件的名称。
        /// </summary>
        [DataMember]
        public string BreadcrumbDocName { get; set; }

        /// <summary>
        /// 一个指向网页的URI，当用户点击BreadcrumbDocName时会跳转到URI指向的地址。
        /// </summary>
        [DataMember]
        public string BreadcrumbDocUrl { get; set; }

        /// <summary>
        /// 表示WOPI向用户展示包含这个文件的文件夹的名称，
        /// </summary>
        [DataMember]
        public string BreadcrumbFolderName { get; set; }

        /// <summary>
        /// 一个指向网页的URI，当用户点击BreadcrumbFolderName时会跳转到URI指向的地址。
        /// </summary>
        [DataMember]
        public string BreadcrumbFolderUrl { get; set; }

        /// <summary>
        /// 一个用户可访问的URI用于通过客户端直接打开文件，它可以是一个DAV URL([RFC5232])，也可以是其他任何能够通过传递类型从而打开文件的其他URL。
        /// </summary>
        [DataMember]
        public string ClientUrl { get; set; }

        /// <summary>
        /// 表示当用户调用了关闭界面操作时，WOPI客户端将关闭浏览器窗口
        /// </summary>
        [DataMember]
        public bool CloseButtonClosesWindow { get; set; }

        /// <summary>
        /// 一个指向网页的URI，表示当用户停止渲染或者编辑客户端当前使用的文件时实施者认对用户有用的网页。
        /// </summary>
        [DataMember]
        public string CloseUrl { get; set; }

        /// <summary>
        /// 表示WOPI客户端必须禁用在浏览器中缓存文件内容。
        /// </summary>
        [DataMember]
        public bool DisableBrowserCachingOfUserContent { get; set; }

        /// <summary>
        /// 表示WOPI客户端在其控制范围内必须禁用打印功能。
        /// </summary>
        [DataMember]
        public bool DisablePrint { get; set; }

        /// <summary>
        /// 表示是WOPI客户端必须禁止使用客户端公开的机器翻译功能。 
        /// </summary>
        [DataMember]
        public bool DisableTranslation { get; set; }

        /// <summary>
        /// 一个用户可访问的指向文件的URI，用户可以通过它下载一个文件的拷贝。
        /// </summary>
        [DataMember]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// 允许用户共享文件位置的URI。
        /// </summary>
        [DataMember]
        public string FileSharingUrl { get; set; }

        /// <summary>
        /// 指向文件位置的URI，WOPI客户端使用它去获得文件，如果提供了这个地址，那么WOPI客户端必须使用它而不是使用“HTTP://server/<...>/wopi*/files/<id>/contents”。
        /// </summary>
        [DataMember]
        public string FileUrl { get; set; }

        /// <summary>
        /// 用于WOPI server唯一标识用户。
        /// </summary>
        [DataMember]
        public string HostAuthenticationId { get; set; }

        /// <summary>
        /// 一个网页的URI，利用WOPI客户端为文件提供编辑的体验。
        /// </summary>
        [DataMember]
        public string HostEditUrl { get; set; }

        /// <summary>
        /// 一个网页的URI，可以嵌入另一个HTML页面并且提供文件的编辑功能。比如该页面提供了以HTML形式插入博客的HTML代码段。
        /// </summary>
        [DataMember]
        public string HostEmbeddedEditUrl { get; set; }

        /// <summary>
        /// 一个网页的URI，可以嵌入另一个HTML页面并且提供文件的预览。比如该页面提供了以HTML形式插入博客的HTML代码段。
        /// </summary>
        [DataMember]
        public string HostEmbeddedViewUrl { get; set; }

        /// <summary>
        /// WOPI服务器提供的名称，用于记录日志或信息。
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// 被WOPI服务器用来向WOPI客户端传递任意信息，WOPI客户端如果不能识别这个信息便有可能会忽略这个字符串。WOPI服务器不能要求WOPI客户端理解这些内容从而进行操作。
        /// </summary>
        [DataMember]
        public string HostNotes { get; set; }

        /// <summary>
        /// 是通过REST方式操作文件最基本的URI。
        /// </summary>
        [DataMember]
        public string HostRestUrl { get; set; }

        /// <summary>
        /// 一个利用WOPI客户端提供预览功能的URI。
        /// </summary>
        [DataMember]
        public string HostViewUrl { get; set; }

        /// <summary>
        /// WOPI客户端应该向用户展示文件的信息权限管理(IRM)策略。这个值应该与IrmPolicyTitle相结合。
        /// </summary>
        [DataMember]
        public string IrmPolicyDescription { get; set; }

        /// <summary>
        /// WOPI客户端应该向用户展示文件的信息权限管理(IRM)策略。这个值应该与IrmPolicyDescription相结合
        /// </summary>
        [DataMember]
        public string IrmPolicyTitle { get; set; }

        /// <summary>
        /// 用于唯一标识文件的所有者。
        /// </summary>
        [DataMember]
        public string OwnerId { get; set; }

        /// <summary>
        /// 识别WOPI客户端可用于发现关于用户的在线状态的信息，比如通过即时的信息判断用户是否在线。WOPI客户端需要知道特定存在的提供者才能够利用这个值。
        /// </summary>
        [DataMember]
        public string PresenceProvider { get; set; }

        /// <summary>
        /// 识别在PresenceProvider上下文中的用户。
        /// </summary>
        [DataMember]
        public string PresenceUserId { get; set; }

        /// <summary>
        /// 一个网页的URI，用于解释WOPI服务器的隐私策略。
        /// </summary>
        [DataMember]
        public string PrivacyUrl { get; set; }

        /// <summary>
        /// 表示WOPI客户端需要对文件的拷贝和打印采取预防措施，它在WOPI客户端中帮助执行IRM。
        /// </summary>
        [DataMember]
        public bool ProtectInClient { get; set; }

        /// <summary>
        /// 提示用户这个文件无法被修改。
        /// </summary>
        [DataMember]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// 表示WOPI客户端一定不允许用户下载文件或者使用单独应用程序打开文件。
        /// </summary>
        [DataMember]
        public bool RestrictedWebViewOnly { get; set; }

        /// <summary>
        /// 它一定是实时且非空的，是256bit的SHA-2-encoded [FIPS180-2] 对于文件内容的散列。
        /// </summary>
        [DataMember]
        public string SHA256 { get; set; }

        /// <summary>
        /// 一个URI用于将当前用户登出WOPI服务器所支持的认证系统。
        /// </summary>
        [DataMember]
        public string SignoutUrl { get; set; }

        /// <summary>
        /// The size of the file expressed in bytes.
        /// </summary>
        [DataMember]
        public long Size { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持多个用户同时对文件进行修改。
        /// </summary>
        [DataMember]
        public bool SupportsCoauth { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持ExecuteCellStorageRequest 和ExcecuteCellStorageRelativeRequest 的操作。
        /// </summary>
        [DataMember]
        public bool SupportsCobalt { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持对于文件的EnumerateChildren和DeleteFile 操作。
        /// </summary>
        [DataMember]
        public bool SupportsFolders { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持对于文件Lock 、Unlock 、RefreshLock 和UnlockAndRelock 操作。
        /// </summary>
        [DataMember]
        public bool SupportsLocks { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持用户可以通过有限的方式对受限制的URL进行操作的场景。
        /// </summary>
        [DataMember]
        public bool SupportsScenarioLinks { get; set; }

        /// <summary>
        /// 表示WOPI服务使用存储在文件中的凭证来调用安全数据存储。
        /// </summary>
        [DataMember]
        public bool SupportsSecureStore { get; set; }

        /// <summary>
        /// 表示WOPI服务器支持对于文件的PutFile 和PutRelativeFile 操作。
        /// </summary>
        [DataMember]
        public bool SupportsUpdate { get; set; }

        /// <summary>
        /// 用于WOPI服务器唯一表示tenant。
        /// </summary>
        [DataMember]
        public string TenantId { get; set; }

        /// <summary>
        /// 一个网页URI，用于解释WOPI服务器的使用条款政策。
        /// </summary>
        [DataMember]
        public string TermsOfUseUrl { get; set; }

        /// <summary>
        /// 用于以WOPI服务器选择的格式将时间区域信息传递给WOPI客户端。
        /// </summary>
        [DataMember]
        public string TimeZone { get; set; }

        /// <summary>
        /// 表示用户有权限查看这个文件的广播。广播是一个文件的活动，涉及控制一组参加者的文件的视图的一个或多个呈现者。比如一个传播者能够通过广播将幻灯片广播给多个接受者。
        /// </summary>
        [DataMember]
        public bool UserCanAttend { get; set; }

        /// <summary>
        /// 表示用户没有足够的权限在WOPI服务器上新建文件。
        /// </summary>
        [DataMember]
        public bool UserCanNotWriteRelative { get; set; }

        /// <summary>
        /// 表示用户有权限广播这个文件给那些有权限浏览文件的人。广播是一个文件的活动，涉及控制一组参加者的文件的视图的一个或多个呈现者。比如一个传播者能够通过广播将幻灯片广播给多个接受者。
        /// </summary>
        [DataMember]
        public bool UserCanPresent { get; set; }

        /// <summary>
        /// 表示用户有权限改变文件。
        /// </summary>
        [DataMember]
        public bool UserCanWrite { get; set; }

        /// <summary>
        /// 用户的名称，如果被锁定，WOPI客户端在某些场景可能会配置一个替代的字符串，或者展示没有名称。
        /// </summary>
        [DataMember]
        public string UserFriendlyName { get; set; }

        /// <summary>
        /// 用于WOPI服务器唯一标识用户。
        /// </summary>
        [DataMember]
        public string UserId { get; set; }

        /// <summary>
        /// 代表基于WOPI服务器的版本模式，文件的当前版本。当文件改变时，这个值一定要改变，同时对于一个给定的文件，版本的值应该从不重复。
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// 表示WOPI客户端不应该允许用户使用WOPI客户端的编辑功能去操作文件，但这不意味着用户没有编辑文件的权限。
        /// </summary>
        [DataMember]
        public bool WebEditingDisabled { get; set; }
    }
}