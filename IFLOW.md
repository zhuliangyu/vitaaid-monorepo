# VitaAid.com 项目文档

## 项目概述

VitaAid.com 是一个基于 .NET 的保健品行业企业级应用系统，采用前后端分离架构。该项目主要为保健品行业提供产品管理、会员管理、治疗重点、治疗协议、网络研讨会等功能。

### 技术栈

#### 后端
- **.NET Framework 4.8** (backend.vitaaid.com - ASP.NET MVC 5)
- **.NET 5.0** (vitaaid.com - ASP.NET Core)
- **NHibernate** 作为 ORM 框架
- **DevExpress** UI 组件库 (v21.1)
- **Entity Framework 6**
- **Microsoft OWIN** 中间件
- **NLog** 日志框架
- **JWT** 身份验证

#### 前端
- **React 18.2.0** 与 TypeScript
- **React Bootstrap** UI 框架
- **Redux Toolkit** 状态管理
- **DevExtreme** React 组件库
- **React Router Dom** 路由管理

#### 数据库
- **Microsoft SQL Server** (支持多个数据库实例)

## 项目结构

```
vitaaid.com-monorepo/
├── backend.vitaaid.com/      # ASP.NET MVC 5 后端应用
├── vitaaid.com/              # ASP.NET Core + React 前端应用
├── CIM.DBPO/                 # 数据库持久化对象
├── CIM.POCO/                 # 数据库领域对象
├── MIS.DBBO/                 # MIS 数据库业务对象
├── MIS.DBPO/                 # MIS 数据库持久化对象
├── WebDB.DBBO/               # Web 数据库业务对象
├── WebDB.DBPO/               # Web 数据库持久化对象
├── ECSServerObj/             # ECS 服务器对象
├── MyHibernateUtil/          # Hibernate 工具类
├── MySystem.Base/            # 基础系统类库
├── MySystem.Base.Web/        # Web 基础系统类库
├── MySystem.Base.win/        # Windows 基础系统类库
├── MyToolkit.Base/           # 基础工具包
├── BouncyCastle/             # 加密相关库
├── WebServiceMiddlewares/    # Web 服务中间件
└── scripts/                  # 脚本文件
```

## 主要功能模块

### 1. 产品管理 (Product Management)
- 产品信息维护
- 产品图片管理
- 产品成分管理
- 产品分类管理
- 产品建议和注意事项编辑

### 2. 会员管理 (Member Management)
- 会员注册与认证
- 会员信息维护
- 执照上传管理
- 会员权限控制

### 3. 治疗重点 (Therapeutic Focus)
- 治疗领域管理
- 相关博客内容
- 参考资料管理

### 4. 治疗协议 (Therapeutic Protocol)
- 治疗方案管理
- PDF 文档上传
- 内容编辑器

### 5. 网络研讨会 (Webinar)
- 研讨会内容管理
- 时间安排
- 资料上传

### 6. 系统分类 (System Category)
- 系统分类管理
- 分类层级维护

## 数据库配置

项目使用多个数据库实例，配置文件位于：
- `va.mis.hibernate.cfg.xml` - VA.MIS 数据库配置
- `webdb.hibernate.cfg.xml` - WebDB 数据库配置
- `cim.hibernate.cfg.xml` - CIM 数据库配置

每个配置文件支持开发和生产环境：
- **开发环境**: 连接到 192.168.1.113\VITAAIDMESPROD
- **生产环境**: 连接到 DBServer\VITAAIDMESPROD

## 构建和运行

### backend.vitaaid.com (ASP.NET MVC 5)
```bash
# 使用 Visual Studio 2022 打开解决方案
# 在 VS2022 中打开 "D:\Code\vitaaid.com\vitaaid.com\vitaaid.com.sln"

# 或使用 MSBuild 命令行
msbuild backend.vitaaid.com.sln /p:Configuration=Debug
```

### vitaaid.com (ASP.NET Core + React)
```bash
# 进入项目目录
cd vitaaid.com

# 还原 NuGet 包
dotnet restore

# 构建项目
dotnet build

# 运行项目
dotnet run
```

### 前端开发
```bash
# 进入前端目录
cd vitaaid.com/ClientApp

# 安装依赖
npm install

# 开发模式运行
npm start

# 构建生产版本
npm run build
```

## 开发注意事项

### 数据库连接
- 连接字符串中的密码已加密，使用 `MyHibernateUtil.DecryptConnectionProvider` 解密
- 开发时注意 `sysconfig.xml` 中的 `DefaultDBFactory` 设置

### 身份验证
- 使用 JWT 进行身份验证
- 密钥配置在 `appsettings.json` 中

### 日志系统
- 使用 NLog 进行日志记录
- 日志文件位置通过 `NLog.GlobalDiagnosticsContext.Set("AppDirectory", path)` 设置

### DevExpress 组件
- 项目大量使用 DevExpress 组件
- 注意版本兼容性 (v21.1)

### 前端路由
- 使用 React Router Dom v6
- 支持懒加载组件 (`@loadable/component`)

## 系统集成

### ECS 服务器集成
- 通过 `ECSServerObj.RESTfullObject` 与 ECS 服务器交互
- 配置文件: `ecsopicfg.ini`

### 文件管理
- 支持多种文件上传 (图片、PDF、文档等)
- 使用 `FileManagerController` 处理文件操作

## 测试与部署

- 使用 NLog 进行错误跟踪和调试
- 支持开发和生产环境配置切换
- 通过 Web.config 或 appsettings.json 管理环境变量

## 安全考虑

- 数据库连接字符串加密
- JWT 身份验证
- 文件上传安全检查
- 用户权限分层管理\
## 语言偏好设置 (Language Preference)

- 用户可以使用英语提问，但 iFlow CLI 必须使用中文回答
- Language Preference: Users can ask in English, but iFlow CLI must respond in Chinese

## Important Instructions for iFlow CLI

- Never edit files without asking first
- Always use plan mode before making changes\
- Explain your reasoning for any suggestions or changes
