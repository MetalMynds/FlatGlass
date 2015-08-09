using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMynds.Utilities
{
    public static class GroupHelper
    {
        public static String GroupHit(Dictionary<String, List<int>> Groups, int Index, bool ExceptionOnFail = true)
        {
            foreach (String groupName in Groups.Keys)
            {
                if (Groups[groupName].Contains(Index))
                {
                    return groupName;
                }
            }

            if (ExceptionOnFail)
            {
                throw new InvalidOperationException(String.Format("Groups Dictionary Must Contain Index Specified! Index: [{0}]", Index));
            }
            else
            {
                return "INDEX_NOT_FOUND_IN_GROUPS";
            }
        }

        public class RolledGroup
        {
            public String Name;
            public List<int> Items;

            public override string ToString()
            {
                if (Items != null)
                {
                    return String.Format("Rolled Group Name: [{0}] Items Count: {1}", Name, Items.Count);
                }
                else
                {
                    return String.Format("Rolled Group Name: [{0}] Items = null", Name);
                }
            }
        }

        public static List<RolledGroup> RollingGroup(String[] Items, bool CaseSensative = false, int IndexOffset = 0)
        {
            List<RolledGroup> rollingGroups = new List<RolledGroup>();

            int index = IndexOffset;

            String previousKey = null;

            RolledGroup currentGroup = null;

            foreach (String item in Items)
            {
                string groupKey;

                if (!CaseSensative)
                {
                    groupKey = item.ToLower();
                }
                else
                {
                    groupKey = item;
                }

                if (groupKey != previousKey)
                {
                    // Start Group
                    currentGroup = new RolledGroup() { Name = item, Items = new List<int>() };

                    rollingGroups.Add(currentGroup);

                    currentGroup.Items.Add(index);
                }
                else
                {
                    currentGroup.Items.Add(index);
                }

                previousKey = groupKey;

                index += 1;
            }

            return rollingGroups;
        }

        public static Dictionary<String, List<int>> RepeatingGroup(String[] Items)
        {
            Dictionary<String, List<int>> grouping = new Dictionary<string, List<int>>();
            Dictionary<String, List<String>> groupLookup = new Dictionary<String, List<String>>();

            int index = 0;

            String previousItem = null;

            string group = null;

            foreach (String item in Items)
            {
                if (group == null)
                {
                    group = String.Empty;
                }
                else if (group == String.Empty)
                {
                    group = "BLANK";
                }

                if (item != previousItem)
                {
                    if (groupLookup.ContainsKey(item))
                    {
                        int repeatCount = groupLookup[item].Count;

                        repeatCount += 1;

                        group = String.Format("{0} ({1})", item, repeatCount);

                        groupLookup[item].Add(group);
                    }
                    else
                    {
                        group = String.Format("{0} (1)", item);

                        groupLookup.Add(item, new List<String>(new string[] { group }));
                    }

                    if (grouping.ContainsKey(group))
                    {
                        grouping[group].Add(index);
                    }
                    else
                    {
                        grouping.Add(group, new List<int>(new int[] { index }));
                    }
                }
                else
                {
                    //if (group != null)
                    //{
                    if (grouping.ContainsKey(group))
                    {
                        grouping[group].Add(index);
                    }
                    else
                    {
                        grouping.Add(group, new List<int>(new int[] { index }));
                    }
                    //}
                }

                previousItem = item;

                index += 1;
            }

            return grouping;
        }
    }
}