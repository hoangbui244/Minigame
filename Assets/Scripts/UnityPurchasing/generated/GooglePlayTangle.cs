// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("px563sVP088+MG4BzbMt/27NA1uMi7H4gi592wz8fc9Tkp/hECK9H58UPGvejfQqJ18nwJiKzvRNXSwFIaKso5MhoqmhIaKiowSspKsPEPs2OKU5ZlAkdxKSs+/rKT8ET8188JMhooGTrqWqiSXrJVSuoqKipqOgKSUDSY5wcUtJwOdBtiubMGPsZ4XT6vHEcYEYLq8x0VoYASG+zGCPkP/3EPxquhbn+cmHaLBpjTv2RVKKVv52CoCdpDzfB5sDQnRtiKlT7o8xZiknPz+B3nTkEVEzP11vNVxmq5e9FXF84S/222VdC1zkGzfyGOudzJtlyAufcmJJVwaAPNsz94oGc7ikA2wJNHmVu60JZHRO5vu2mJzb0Wql3HAllW44SKGgoqOi");
        private static int[] order = new int[] { 3,9,6,9,7,9,12,11,8,11,11,11,12,13,14 };
        private static int key = 163;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
