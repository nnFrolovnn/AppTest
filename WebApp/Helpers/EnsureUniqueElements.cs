using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Helpers
{
    public class EnsureUniqueElements : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                for(int i=0; i < list.Count-1; i++)
                {
                    for(int j=i+1; j < list.Count; j++)
                    {
                        if(list[i] == list[j])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}