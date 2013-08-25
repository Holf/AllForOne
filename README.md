AllForOne
=========
**Put an end to pesky hanging processes on your Build Server.**

What does AllForOne do?
-----------------------
AllForOne ensures that any processes which are started by your programs will end when you program ends.

Why is this useful?
-------------------
Integration Test Fixtures often use browser automation frameworks, such as Selenium or Watin, to start independent browser processes. Sometimes an intermediary process, which acts as a shim between the browser and the Browser Automation Framework, also needs to be started.

For example, to automate browser testing using Google Chrome and Selenium, your Integration Test Fixture Setup code will need to start an instance of Chrome and also an instance of the Selenium Chrome Driver.

Your Test Fixture Teardown will (hopefully!) contain code which kills any processes which were started in your Test Fixture Setup. This is important, as otherwise each time you run your tests you'll start another browser instance, and another, and another...

So, you've got code in your Test Fixture Teardown to sort this out, so there's no problem, right? True... *but only if your Test Fixture Teardown gets to run*.

###Why might your Test Fixture Teardown not run?

1. **You abort a test run**

    You're running your integration tests in Visual Studio. You realise something's not right so you click on the `Stop` button in your test runner. Your tests stop, but the Test Fixture Teardown code never had a chance to run. Therefore, your test browser instance is still running.
  
    Okay, this is not the end of the world. You can see the test browser is still running and you can kill it. But what a hassle, having to kill the test browser (and possibly something else like Chrome Driver) every time you stop a test run!
    
    And sometimess you start the test run again, forgetting to kill the test browser and only remembering when your test code complains that a browser is already running. So you stop the tests again, and have to close the original test browser, and the second one that just got started, and maybe two Chrome Driver instances too...
    
2. **Your test run crashes on your Build Server**
 
    This is potentially a bit more of an issue. Integration test runs crash on Build Servers for all sorts of reasons. Perhaps there is a 32 vs 64 bit mismatch. Maybe there is a permissions problem. Whatever the cause, you really don't want any test related processes hanging around after a build has stopped.

   If you're using Continuous Integration your integration tests will be running on your Build Server automatically, hopefully when you check code in several times a day. If something is going wrong with each run, you could end up with tens, if not hundreds of test browser instances running on your Build Server. You'll only know there's a problem the next time you log on and see a screen filled with browsers, or when a co-worker complains that you're using up all the Build Server resources with your crazy Integration Tests.
   
How does AllForOne work?
------------------------

AllForOne uses Job Object voodoo to ensure a set of processes are managed as a unit. More detail can be found here: http://msdn.microsoft.com/en-us/library/ms684161(v=vs.85).aspx#managing_job_objects.

This StackOverflow answer inspired much of the code: http://stackoverflow.com/a/4657392/169334.

However, the 'LimitFlags' setting used is '0x3000' rather than '0x2000'. This applies 'JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK', as well as 'JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE'. Without it, any child processes spawned, such as by instances of Chrome, get assigned and locked to the same Job. Chrome cannot thereafter use it's own Job Management strategies, and therefore crashes.


  
