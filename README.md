# 🏗️ Trockenbau - Modern Project Management System

[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Desktop-0078D4?style=for-the-badge&logo=windows)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows-0078D4?style=for-the-badge&logo=windows)](https://www.microsoft.com/windows)

A modern WPF application for comprehensive construction project management, built with .NET 6.0 and Material Design principles.

## ✨ Features

### 🔐 **Authentication & Security**
- **Role-based access control** with multiple user roles
- **Secure login system** with role-based permissions
- **User management** with administrative controls

### 📊 **Core Management Modules**
- **👥 Customer Management** - Complete customer data and contact management
- **👤 User Management** - System user administration and permissions
- **🏗️ Construction Site Management** - Project tracking and status management
- **📋 Quote Management** - Professional quote creation and tracking
- **📏 Measurement Management** - Survey data capture and management
- **💰 Invoice Management** - Complete billing and invoicing system
- **📋 Performance Catalog (LV)** - Service catalog management
- **ℹ️ About** - Application information and support

### 🎨 **Modern User Interface**
- **Material Design** components and styling
- **Responsive layout** with tab-based navigation
- **Dark/Light themes** support
- **Intuitive user experience** with modern UI patterns

## 🛠️ Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET** | 6.0 | Modern framework |
| **WPF** | Latest | Desktop UI framework |
| **Material Design** | Latest | Modern UI components |
| **MVVM Pattern** | - | Clean architecture |
| **Entity Framework** | Core 6.0 | Data access |
| **SQLite** | Latest | Local database |

## 🚀 Quick Start

### Prerequisites
- **Windows 10/11**
- **.NET 6.0 SDK** or higher
- **Visual Studio 2022** or **JetBrains Rider**

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Viktoriusssss/Trockenbau.git
   cd Trockenbau
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## 🔑 Login

To access the system, please contact your system administrator for login credentials.

**Note:** Only administrators can create new user accounts.

## 📁 Project Structure

```
Trockenbau/
├── 📁 Models/                 # Data models and entities
│   ├── User.cs               # User management
│   ├── Kunde.cs              # Customer data
│   ├── Baustelle.cs          # Construction site
│   ├── Angebot.cs            # Quote management
│   ├── Aufmass.cs            # Measurement data
│   ├── Rechnung.cs           # Invoice management
│   └── LVPosition.cs         # Performance catalog
├── 📁 Services/              # Business logic layer
│   ├── AuthenticationService.cs
│   └── DatabaseService.cs
├── 📁 ViewModels/            # MVVM ViewModels
│   ├── LoginViewModel.cs
│   ├── MainWindowViewModel.cs
│   ├── KundenViewModel.cs
│   ├── BaustelleViewModel.cs
│   └── [Other ViewModels...]
├── 📁 Views/                 # UI Views and Windows
│   ├── LoginWindow.xaml
│   ├── KundenView.xaml
│   ├── BaustellenView.xaml
│   ├── AngeboteView.xaml
│   ├── AufmassView.xaml
│   ├── RechnungenView.xaml
│   ├── LVView.xaml
│   └── AboutView.xaml
├── 📁 Data/                  # Database context
│   └── ApplicationDbContext.cs
├── 📁 Converters/            # Value converters
├── 📁 Migrations/            # Database migrations
├── MainWindow.xaml           # Main application window
├── App.xaml                  # Application configuration
└── ModernWPFApp.csproj       # Project file
```

## 🏗️ Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **📊 Models**: Data structures and business objects
- **🖥️ Views**: XAML-based user interfaces
- **🔗 ViewModels**: Binding logic between Models and Views
- **⚙️ Services**: Business logic and data processing

## 🎯 Key Features in Detail

### Customer Management
- Complete customer database
- Contact information management
- Project history tracking
- Search and filter capabilities

### Construction Site Management
- Project creation and tracking
- Status management
- Timeline visualization
- Resource allocation

### Quote & Invoice System
- Professional quote generation
- Invoice creation and management
- PDF export capabilities
- Payment tracking

### Measurement System
- Survey data capture
- Measurement calculations
- Report generation
- Data validation

## 🔧 Development

### Adding New Features

1. **Create Models** in the `Models/` folder
2. **Implement Services** in the `Services/` folder
3. **Create ViewModels** in the `ViewModels/` folder
4. **Design Views** in the `Views/` folder
5. **Register new tabs** in `MainWindow.xaml`

### Code Style
- Follow MVVM pattern
- Use async/await for operations
- Implement proper error handling
- Add XML documentation

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Development Guidelines
- Follow existing code style
- Add tests for new features
- Update documentation
- Ensure all tests pass

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

Need help? Here's how to get support:

- **🐛 Report Bugs**: Create an issue in the repository
- **💡 Feature Requests**: Submit a feature request
- **📧 Contact**: Reach out to the development team
- **📖 Documentation**: Check the code comments and XML docs

## 📈 Roadmap

- [ ] **Mobile companion app**
- [ ] **Cloud synchronization**
- [ ] **Advanced reporting**
- [ ] **Multi-language support**
- [ ] **API integration**
- [ ] **Advanced analytics**

## 🙏 Acknowledgments

- **Material Design** for the beautiful UI components
- **Microsoft** for the excellent .NET platform
- **Community** for the helpful libraries and tools

---

<div align="center">

**Built with ❤️ using .NET 6.0 and WPF**

[![GitHub stars](https://img.shields.io/github/stars/Viktoriusssss/Trockenbau?style=social)](https://github.com/Viktoriusssss/Trockenbau/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/Viktoriusssss/Trockenbau?style=social)](https://github.com/Viktoriusssss/Trockenbau/network/members)
[![GitHub issues](https://img.shields.io/github/issues/Viktoriusssss/Trockenbau)](https://github.com/Viktoriusssss/Trockenbau/issues)

**Version**: 1.0.0 | **Last Updated**: 2024

</div>