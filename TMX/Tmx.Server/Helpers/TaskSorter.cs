﻿/*
 * Created by SharpDevelop.
 * User: Alexander Petrovskiy
 * Date: 7/22/2014
 * Time: 8:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace Tmx.Server
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Tmx.Interfaces.Remoting;
	
	/// <summary>
	/// Description of TaskSorter.
	/// </summary>
	public class TaskSorter
	{
	    public List<ITestTask> GetTasksForClient(int clientId)
	    {
	        var result = new List<ITestTask>();
	        
	        // temporarily!!!!
	        // TODO: add sorting code by rules
	        result = TaskPool.Tasks; //.Where(task => task.
	        
	        return result;
	    }
	}
}