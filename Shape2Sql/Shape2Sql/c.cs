namespace Shape2Sql
{
    using System;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Text.RegularExpressions;

    [DefaultMember("Item")]
    public class c
    {
        private StringDictionary a = new StringDictionary();

        public c(string[] A_0)
        {
            Regex regex = new Regex("^-{1,2}|^/|=|:", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("^['\"]?(.*?)['\"]?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string key = null;
            foreach (string str2 in A_0)
            {
                string[] strArray = regex.Split(str2, 3);
                switch (strArray.Length)
                {
                    case 1:
                        if (key != null)
                        {
                            if (!this.a.ContainsKey(key))
                            {
                                strArray[0] = regex2.Replace(strArray[0], "$1");
                                this.a.Add(key, strArray[0]);
                            }
                            key = null;
                        }
                        break;

                    case 2:
                        if ((key != null) && !this.a.ContainsKey(key))
                        {
                            this.a.Add(key, "true");
                        }
                        key = strArray[1];
                        break;

                    case 3:
                        if ((key != null) && !this.a.ContainsKey(key))
                        {
                            this.a.Add(key, "true");
                        }
                        key = strArray[1];
                        if (!this.a.ContainsKey(key))
                        {
                            strArray[2] = regex2.Replace(strArray[2], "$1");
                            this.a.Add(key, strArray[2]);
                        }
                        key = null;
                        break;
                }
            }
            if ((key != null) && !this.a.ContainsKey(key))
            {
                this.a.Add(key, "true");
            }
        }

        public string a(string A_0)
        {
            return this.a[A_0];
        }
    }
}

