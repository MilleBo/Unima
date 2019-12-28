﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Testura.Mutation.Core.Creator.Filter
{
    public class MutationDocumentFilterItem
    {
        public enum FilterEffect
        {
            Allow,
            Deny
        }

        public string Resource { get; set; }

        public IList<string> Lines { get; set; }

        public FilterEffect Effect { get; set; }

        public bool MatchResource(string resource)
        {
            return Regex.IsMatch(resource.Replace('\\', '/'), FormattedResource(), RegexOptions.IgnoreCase);
        }

        public bool IsDenied(string resource)
        {
            return CheckResource(FilterEffect.Deny, resource);
        }

        public bool IsAllowed(string resource)
        {
            return CheckResource(FilterEffect.Allow, resource);
        }

        public virtual bool MatchFilterLines(int line)
        {
            var lineNumbers = GetLineNumbers();

            if (!lineNumbers.Any())
            {
                return true;
            }

            return lineNumbers.Contains(line);
        }

        public bool LineAreDenied(int line)
        {
            return MatchFilterLines(line) && Effect == FilterEffect.Deny;
        }

        public bool LineAreAllowed(int line)
        {
            return MatchFilterLines(line) && Effect == FilterEffect.Allow;
        }

        private IList<int> GetLineNumbers()
        {
            if (Lines == null)
            {
                return Array.Empty<int>();
            }

            var lines = new List<int>();

            foreach (var line in Lines)
            {
                if (line.Contains(","))
                {
                    var startAndCount = line.Split(',');
                    var start = int.Parse(startAndCount[0]);
                    var length = int.Parse(startAndCount[1]);

                    for (int n = 0; n <= length; n++)
                    {
                        lines.Add(start + n);
                    }

                    continue;
                }

                lines.Add(int.Parse(line));
            }

            return lines;
        }

        private bool CheckResource(FilterEffect effect, string resource)
        {
            var isMatch = MatchResource(resource);
            return isMatch && Effect == effect;
        }

        private string FormattedResource()
        {
            var formattedResource = Resource.Replace('\\', '/');
            return "^" + Regex.Escape(formattedResource).Replace("\\*", ".*") + "$";
        }
    }
}
