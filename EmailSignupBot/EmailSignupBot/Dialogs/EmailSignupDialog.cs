// The MIT License(MIT)
//
// Copyright(c) Applied information Sciences
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmailSignupBot.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace EmailSignupBot.Dialogs
{
    // Update this attribute with your LUIS model App ID and Key
    [LuisModel("00000000-0000-0000-0000-000000000000", "00000000000000000000000000000000")]
    [Serializable]
    public class EmailSignupDialog : LuisDialog<object>
    {
        /// <summary>
        /// Handles the default "None" LUIS intent
        /// </summary>
        /// <param name="context">The dialog context</param>
        /// <param name="result">The result of LUIS parsing</param>
        /// <returns></returns>
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry, I didn't understand.");
            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Handles the "Greet" LUIS intent
        /// </summary>
        /// <param name="context">The dialog context</param>
        /// <param name="result">The result of LUIS parsing</param>
        /// <returns></returns>
        [LuisIntent("Greet")]
        public async Task Greet(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello! Welcome to the email sign-up bot. What would you like to do?");
            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Handles the "SignUp" LUIS intent
        /// </summary>
        /// <param name="context">The dialog context</param>
        /// <param name="result">The result of LUIS parsing</param>
        /// <returns></returns>
        [LuisIntent("SignUp")]
        public async Task SignUp(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Great! I just need a few pieces of information to get you signed up.");

            var form = new FormDialog<SignupForm>(
                new SignupForm(),
                SignupForm.BuildForm,
                FormOptions.PromptInStart,
                PreprocessEntities(result.Entities));

            context.Call<SignupForm>(form, SignUpComplete);
        }

        /// <summary>
        /// Handles when the SignupForm is complete.
        /// </summary>
        /// <param name="context">The dialog context</param>
        /// <param name="result">The completed form</param>
        /// <returns></returns>
        private async Task SignUpComplete(IDialogContext context, IAwaitable<SignupForm> result)
        {
            SignupForm form = null;
            try
            {
                form = await result;
            }
            catch (OperationCanceledException)
            {
            }

            if (form == null)
            {
                await context.PostAsync("You canceled the form.");
            }
            else
            {
                // Here is where we could call our signup service here to complete the sign-up

                var message = $"Thanks! We signed up **{form.EmailAddress}** in zip code **{form.ZipCode}**.";
                await context.PostAsync(message);
            }

            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Preprocesses any entities sent from LUIS parsing.
        /// </summary>
        /// <param name="entities">The entities to processed</param>
        /// <returns>The processed entities</returns>
        private IList<EntityRecommendation> PreprocessEntities(IList<EntityRecommendation> entities)
        {
            // remove spaces from email address
            var emailEntity = entities.Where(e => e.Type == "EmailAddress").FirstOrDefault();
            if (emailEntity != null)
            {
                emailEntity.Entity = Regex.Replace(emailEntity.Entity, @"\s+", string.Empty);
            }
            return entities;
        }
    }
}