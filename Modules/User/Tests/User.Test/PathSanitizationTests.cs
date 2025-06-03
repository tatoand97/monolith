using NameProject.Server.Utils;
using Xunit;

namespace User.Test;

public class PathSanitizationTests
{
    [Theory]
    [InlineData("/path//double")]
    [InlineData("../etc/passwd")]
    [InlineData("/file$")]
    [InlineData("/path/{var}")]
    [InlineData("/abc%0")]
    [InlineData("/auth/user")]
    [InlineData("/thishouldnotexistandhope")]
    public void IsMalicious_ReturnsTrue_ForMaliciousPaths(string path)
    {
        Assert.True(PathSanitization.IsMalicious(path));
    }

    [Theory]
    [InlineData("/api/user")]
    [InlineData("/some/path")]
    [InlineData("/")]
    [InlineData("/users/profile")]
    [InlineData("/auth/userinfo")]
    public void IsMalicious_ReturnsFalse_ForSafePaths(string path)
    {
        Assert.False(PathSanitization.IsMalicious(path));
    }
}
