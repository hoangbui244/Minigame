// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("ELfYvYDNIQ8ZvdDA+lJPAiwob2UjCaHFyFWbQm/R6b/oUK+DRqxfKWdeRXDFNayaG4Vl7qy1lQp41DsknZG3/TrExf/9dFP1Ap8vhNdY0zHiSsK+NCkQiGuzL7f2wNk8HedaO3gv0Xy/K8bW/eOyNIhvh0M+sscME6rOanH7Z3uKhNq1eQeZS9p5t+8roIjfajlAnpPrk3QsPnpA+emYsYXSnZOLizVqwFCl5YeL6duB6NIflRYYFyeVFh0VlRYWF7AYEB+7pE9LQ6RI3g6iU019M9wE3TmPQvHmPjg/BUw2mslvuEjJe+cmK1WklgmrgowRjdLkkMOmJgdbX52LsPt5yEQnlRY1JxoRHj2RX5HgGhYWFhIXFN4RaMSRIdqM/BUUFhcW");
        private static int[] order = new int[] { 13,10,11,12,4,11,12,12,11,10,11,11,12,13,14 };
        private static int key = 23;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
