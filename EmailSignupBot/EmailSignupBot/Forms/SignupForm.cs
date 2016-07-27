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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace EmailSignupBot.Forms
{
    /// <summary>
    /// Implements a Form and FormFlow to collect email address and zip code for sign up.
    /// </summary>
    [Serializable]
    public class SignupForm
    {
        public string EmailAddress { get; set; }
        public string ZipCode { get; set; }

        // email regex is from: https://html.spec.whatwg.org/multipage/forms.html#valid-e-mail-address
        private const string EmailRegExPattern = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        private const string ZipRegExPattern = @"^([0-9]{5})(?:[-\s]*([0-9]{4}))?$";

        /// <summary>
        /// Email address validator
        /// </summary>
        private static ValidateAsyncDelegate<SignupForm> EmailValidator = async (state, response) =>
        {
            var result = new ValidateResult { IsValid = true, Value = response };
            var email = (response as string).Trim();
            if (!Regex.IsMatch(email, EmailRegExPattern))
            {
                result.Feedback = "Sorry, that doesn't look like a valid email address.";
                result.IsValid = false;
            }

            return await Task.FromResult(result);
        };

        /// <summary>
        /// Zip code validator
        /// </summary>
        private static ValidateAsyncDelegate<SignupForm> ZipValidator = async (state, response) =>
        {
            var result = new ValidateResult { IsValid = true, Value = response };
            var zip = (response as string).Trim();
            if (!Regex.IsMatch(zip, ZipRegExPattern))
            {
                result.Feedback = "Sorry, that is not a valid zip code. A zip code should be 5 digits.";
                result.IsValid = false;
            }

            return await Task.FromResult(result);
        };

        /// <summary>
        /// Builds the Signup form.
        /// </summary>
        /// <returns>An instance of the <see cref="SignupForm"/> form flow.</returns>
        public static IForm<SignupForm> BuildForm()
        {
            return new FormBuilder<SignupForm>()
                .Field(nameof(SignupForm.EmailAddress), validate: EmailValidator)
                .Field(nameof(SignupForm.ZipCode), validate: ZipValidator)
                .Build();
        }
    }
}