# Email Signup Bot

This sample from [Applied Information Sciences](http://www.appliedis.com/)
implements a simple bot that helps a user sign up for a fictitious email
notification service by collecting an email address and zip code.

For details, read the accompanying blog articles about the
[Bot Framework](https://blog.appliedis.com/2016/07/26/the-bot-framework/) and
[building a bot](https://blog.appliedis.com/2016/07/27/building-a-conve…he-bot-framework/).

To run this bot locally in Visual Studio 2015 (with the latest updates):

1. Go to the [LUIS website](https://www.luis.ai)
   and createa an acccount if you do not already have one.

2. Import a new application (currently the New App -> Import Existing Application menu).

3. Browse to the LUIS\EmailSignupBot.json file in this project and import it.

4. Open your new LUIS app, and under App Settings copy the App ID and subscription Key.

5. In the `EmailSignupDialog` class update the `LuisModel` attribute
   with your LUIS App ID and Key.

You can now build and run the sample locally, then connect using the
[Bot Framework Emulator](https://docs.botframework.com/en-us/tools/bot-framework-emulator/).

