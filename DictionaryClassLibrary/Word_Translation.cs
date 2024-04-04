using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Timelon.Data;

namespace DictionaryClassLibrary
{
    /*public class Word_Translation : Unique<Word_Translation>, IUniqueIdentifiable
    {
        /// <summary>
        /// Базовый констурктор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="theme"></param>
        /// <param name="word"></param>
        /// <param name="translation"></param>
        public Word_Translation(int id,string theme, string word,string translation ) : base(id,theme)
        {
           //PASS
        }


    }*/
    public class Word_Translation /*: Unique*/
    {
        private string word;
        private string translation;

        public string Word { get => word; set => word = value; }
        public string Translation { get => translation; set => translation = value; }


        public Word_Translation(string word, string translation)
        {
            this.Word = word;
            this.Translation = translation;
        }
        public Word_Translation()
        {
            this.Word = string.Empty;
            this.Translation = string.Empty;
        }
    }

}
