﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TownOfSushi.CustomOption
{
    public class CustomTabOption : CustomButtonOption
    {
        public List<CustomOption> InternalOptions = new List<CustomOption>();
        private int Index { get; set; }
        protected internal CustomTabOption(int id, MultiMenu menu, string name) : base(id, menu, name)
        {
            Index = -1;
        }

        public void addOption(CustomTabOption option)
        {
            InternalOptions.Add(option);
        }

        public int getNextID()
        {
            Index++;
            return Index;
        }
    }
}
