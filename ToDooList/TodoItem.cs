﻿using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace ToDooList
{
	public class TodoItem
	{
		string id;
		string task;
        string price;
        string childrensEmail;
        string parentsEmail;
        string i_imageSource;
        bool taskready;
        bool done;
        bool softdelete;

        [JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}

		[JsonProperty(PropertyName = "Task")]
		public string Task
		{
			get { return task; }
			set { task = value;}
		}

        [JsonProperty(PropertyName = "Price")]
        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        [JsonProperty(PropertyName = "ParentsEmail")]
        public string ParentsEmail
        {
            get { return parentsEmail; }
            set { parentsEmail = value; }
        }

        [JsonProperty(PropertyName = "ChildrensEmail")]
        public string ChildrensEmail
        {
            get { return childrensEmail; }
            set { childrensEmail = value; }
        }

        [JsonProperty(PropertyName = "TaskReady")]
        public bool TaskReady
        {
            get { return taskready; }
            set { taskready = value; }
        }

        [JsonProperty(PropertyName = "SoftDelete")]
        public bool SoftDelete
        {
            get { return softdelete; }
            set { softdelete = value; }
        }

        [JsonProperty(PropertyName = "complete")]
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }

        [JsonProperty(PropertyName = "imageSource")]
        public string imageSource
        {
            get { return i_imageSource; }
            set { i_imageSource = value; }
        }

        [Version]
        public string Version { get; set; }
	}
}

