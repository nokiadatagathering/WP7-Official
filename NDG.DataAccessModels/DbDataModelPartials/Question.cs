using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using NDG.DataAccessModels.DataModels;

namespace NDG.DataAccessModels
{
    public partial class Question
    {
        /// <summary>
        /// Stores concrete question information. Creates from XML property based on Type property
        /// </summary>
        private QuestionData _data;
        public QuestionData Data
        {
            get
            {
                if (_data == null)
                {
                    _data = QuestionDataFactory.GetQuestionDataForQuestion(this);
                }
                return _data;
            }
        }

        public void UpdateDependentQuestionsData()
        {
            foreach (var depQuestion in this.DependentQuestions)
            {
                foreach (var innerDepQuestion in depQuestion.Question_DependentQuestions)
                {
                    innerDepQuestion.Data.IsEnabled = this.Data.GetResult().Equals(depQuestion.RequiredAnswer);
                    
                }
            }
        }
    }
}
