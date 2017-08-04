﻿using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using SQLiteDemo.Data;
using SQLiteDemo.Models;
using SQLiteDemo.Views;
using Xamarin.Forms.CommonCore;

namespace SQLiteDemo.ViewModels
{
    public class AppViewModel: ObservableViewModel
    {
        private IDisposable queryToken;
        private Person newPerson;
        private ObservableCollection<Person> people;
        private SQLiteData _data = new SQLiteData();

        public Person NewPerson
        {
            get { return newPerson ?? (newPerson = new Person()); }
            set { SetProperty(ref newPerson, value); }
        }

        public ObservableCollection<Person> People
        {
            get { return people ?? (people = new ObservableCollection<Person>()); }
			set { SetProperty(ref people, value); }
        }

        public ICommand AddPerson { get; set; }
        public ICommand ViewPeople { get; set; }

        public AppViewModel()
        {
 
            People = _data.GetAllPersons().ToObservable();

			//queryToken= _data.GetAllPersons().SubscribeForNotifications((sender, changes, error) =>
   //         {
   //             People = RealmDb.All<Person>().ToObservable();
   //         });

            AddPerson = new RelayCommand(async (obj) => {

				//RealmDb.Write(() =>
    //            {
    //                RealmDb.Add(NewPerson);
    //                NewPerson = new Person();
    //            });

                await Navigation.PushAsync(new PageTwo());
            });

            ViewPeople = new RelayCommand(async(obj) => { 
                await Navigation.PushAsync(new PageTwo());
            });

        }

        ~AppViewModel(){
            queryToken.Dispose();
        }

        public override void OnViewMessageReceived(string key, object obj)
        {
            if(key=="deletePerson" && obj!=null)
            {
                var pk = (string)obj;
                var item = _data.GetAllPersons().FirstOrDefault(x => x.Id == pk);
                if (item != null)
                {
                    _data.DeletePerson(item);
                }

            }
        }

    }
}