using System.Text;

namespace C_Fundamentals.ConsoleApp.TrainingApps.PasswordManager;
internal static class Encryption
{
    private static string _originalPass = "AB~>?+<{}[]$#!@^&*().,'CDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234-567_89";
    private static string _fakePass = "b.cd$efg#hi<jk@lmnopqrst^uv]w&xyz!AB[CD{EF}GH(I)JKL,MNO'PQR-STUVW*X+YZ1?>234_567>890a";

    public static string Encrypt(string password)
    {
        var sb = new StringBuilder();

        foreach (var ch in password)
        {
            sb.Append(_fakePass[_originalPass.IndexOf(ch)]);
        }
        return sb.ToString();
    }
    public static string Decrypt(string password)
    {
        var sb = new StringBuilder();
        foreach (var ch in password)
        {
            sb.Append(_originalPass[_fakePass.IndexOf(ch)]);
        }
        return sb.ToString();
    }
}
