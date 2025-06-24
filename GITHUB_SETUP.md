# GitHub Repository Setup Guide

## Step 1: Create Your GitHub Repository

1. Go to [github.com](https://github.com) and sign in
2. Click the "+" icon in the top right → "New repository"
3. Repository settings:
   - **Repository name**: `gta6-countdown` (or your preferred name)
   - **Description**: "GTA 6 Countdown Desktop Application"
   - **Visibility**: ✅ Private (keeps your code private)
   - **Initialize**: ✅ Add a README file
   - Click "Create repository"

## Step 2: Configure the UpdateService

After creating your repository, you need to update the UpdateService.cs file:

1. Open `Services/UpdateService.cs`
2. Find this line:
   ```csharp
   private const string GITHUB_API_URL = "https://api.github.com/repos/YOUR_USERNAME/YOUR_REPO_NAME/releases/latest";
   ```
3. Replace `YOUR_USERNAME` with your GitHub username
4. Replace `YOUR_REPO_NAME` with your repository name
5. Example:
   ```csharp
   private const string GITHUB_API_URL = "https://api.github.com/repos/johnsmith/gta6-countdown/releases/latest";
   ```

## Step 3: Upload Your Code

### Option A: Using GitHub Desktop (Recommended for beginners)
1. Download [GitHub Desktop](https://desktop.github.com/)
2. Clone your repository locally
3. Copy your project files to the cloned folder
4. Commit and push changes

### Option B: Using Git Command Line
```bash
cd "C:\Users\joser\source\repos\GTA 6 Countdown"
git init
git remote add origin https://github.com/YOUR_USERNAME/YOUR_REPO_NAME.git
git add .
git commit -m "Initial commit"
git branch -M main
git push -u origin main
```

## Step 4: Create Your First Release

1. Build your application for release:
   ```
   dotnet publish -c Release --self-contained false --runtime win-x64
   ```

2. Go to your GitHub repository page
3. Click "Releases" → "Create a new release"
4. Release settings:
   - **Tag version**: `v1.0.0` (include the 'v' prefix)
   - **Release title**: `GTA 6 Countdown v1.0.0`
   - **Description**: Add release notes
   - **Attach files**: Upload your `.exe` from `bin\Release\net8.0-windows\publish\`
   - **Release type**: ✅ Set as the latest release
   - **Visibility**: ✅ Public (allows app to download updates)

## Step 5: Test the Updater

1. Run your application
2. Right-click → "Check for Updates"
3. It should show "You are running the latest version"

## Creating Future Updates

1. Update version in `GTA 6 Countdown.csproj`:
   ```xml
   <Version>1.1.0</Version>
   <AssemblyVersion>1.1.0</AssemblyVersion>
   <FileVersion>1.1.0</FileVersion>
   ```

2. Build and create new release with higher version number
3. Your existing users will automatically be notified of the update

## Important Notes

- Your repository is private, but releases must be public for the updater to work
- The app checks for updates on startup and when manually requested
- Users can choose to download updates or ignore them
- The updater automatically handles file replacement and app restart
