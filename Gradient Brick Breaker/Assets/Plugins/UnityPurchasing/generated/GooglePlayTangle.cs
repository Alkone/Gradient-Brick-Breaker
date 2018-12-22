#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("7MY4kxdoA94CQK/W0Fd+MJkM2S17jv5r+dGUhyIAWp7WLNShQxYOIl6KKu0SFV90OK0ihnF+EaSmNOLCcvH/8MBy8frycvHx8HLr/7OQKan7190BBtY08Fa4nwPeLfvp+bgV0AuxeWUDSSar4zRff29NQh/h4EhOwHLx0sD99vnadrh2B/3x8fH18PPwX29TZkUSqjRO2fKoDvMCWzLYSb2JAOHiTb4oD1gqz+ElEseJSSWjmudUXK9OiqQrMyOC0gjMbumjpnCe1Wq+jKOBSoj3egXS4ZI5dRMyXprj5r0/cVbDEfcXyXGzeoGKvTSy9eyZMRKLeJ23sEqpKE1w6psjtP5atD3yVP8SOxwOPm1qUWRdBvB4C2FOHjFwtldajfLz8fDx");
        private static int[] order = new int[] { 8,9,6,9,7,7,8,9,11,13,11,12,12,13,14 };
        private static int key = 240;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
