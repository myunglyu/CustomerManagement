# Woori Optical Customer Management

This is a production-ready, secure customer management system for optical stores, built with ASP.NET Core MVC, Entity Framework Core (SQLite), and Identity. It includes a WinForms + WebView2 desktop host and PWA support.

## Features
- Customer, order, and prescription management
- Secure authentication with admin account
- Admin account seeded from `admin.json` at startup
- Local-only access (no remote connections allowed)
- PWA (Progressive Web App) support
- WinForms + WebView2 desktop host
- Single-file, self-contained deployment
- Print support
- **Production Features:**
  - Structured logging with event log support
  - Global error handling and custom error pages
  - Comprehensive input validation
  - Response caching for improved performance
  - Database backup and restore functionality
  - Health check endpoints
  - Environment-specific configuration

## Production Readiness

This application includes enterprise-level features for production deployment:

### 🔒 Security
- Input validation and sanitization
- CSRF protection on all forms
- Secure password policies
- Local-only access restrictions

### 📊 Monitoring & Logging
- Structured logging with console, debug, and Windows Event Log
- Health check endpoint at `/health`
- Error tracking with unique request IDs
- Performance monitoring capabilities

### 🔄 Data Management
- Automated database backup service
- Data validation with comprehensive error messages
- Transaction handling for data integrity

### ⚡ Performance
- Response caching middleware
- Memory caching for frequently accessed data
- Optimized database queries
- Asset optimization

### 🧪 Testing
- Unit test project structure included
- Test framework ready for business logic testing

## Getting Started

### Prerequisites
- .NET 9.0 SDK or newer
- Windows 10/11 (x64)

### Setup
1. **Clone the repository**
2. **Configure the admin account**
   - Edit `WooriOptical/admin.json` with your desired admin credentials:
     ```json
     {
       "UserName": "admin",
       "Password": "YourStrongPassword!",
       "Email": "admin@example.com"
     }
     ```
   - Password can be changed later in the app.
3. **Restore and build the solution**
   ```powershell
   dotnet restore
   dotnet build
   ```
4. **Recreate the database (optional, for a fresh start)**
   ```powershell
   dotnet ef database drop -f --project WooriOptical/WooriOptical.csproj
   dotnet ef database update --project WooriOptical/WooriOptical.csproj
   ```
5. **Run the backend app**
   ```powershell
   dotnet run --project WooriOptical/WooriOptical.csproj
   ```
   - The app will be available at `https://localhost:5001` (or as configured).
   - Only local access is allowed.

6. **Run the desktop app**
   ```powershell
   dotnet run --project WooriOptical.Desktop/WooriOptical.Desktop.csproj
   ```

### Deployment
- Use the following command to publish as a single-file, self-contained executable:
  ```powershell
  dotnet publish WooriOptical/WooriOptical.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
  dotnet publish WooriOptical.Desktop/WooriOptical.Desktop.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
  ```
- Copy the contents of the `publish` folders to your deployment directory (e.g., `Customer Management`).

## Notes
- The admin account is seeded only if it does not already exist.
- The backend will refuse all non-local requests.

---

© 2025 Woori Optical. All rights reserved.
