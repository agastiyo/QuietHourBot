using DSharpPlus.Entities;
using QuietHourBot.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuietHourBot.Databases
{
    [Serializable]
    public class NoteDB
    {
        private readonly BinaryFormatter formatter;
        private const string DATA_FILENAME = "notedb.dat";

        public List<Note> notes = new List<Note>();

        public NoteDB()
        {
            this.formatter = new BinaryFormatter();
        }

        public void AddNote(Note note, DiscordMember caller)
        {
            if(note.Content == null || note.Content == " ")
            {
                throw new NoteNotValidException(note.Content);
            }
            if(!IndexNotes(caller))
            {
                throw new TooManyNotesException();
            }
            notes.Add(note);
        }

        /// <summary>
        /// Deletes note with given keywords.
        /// </summary>
        /// <param name="notePart"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public void DeleteNote(string notePart, DiscordMember caller)
        {
            List<Note> tempStorage = new List<Note>();

            foreach (Note note in notes)
            { 
                if (note.AuthorID == caller.Id && note.Content.Contains(notePart))
                {
                    tempStorage.Add(note);
                }
            }

            if (tempStorage.Count > 1) { throw new MoreThanOneNoteFoundException(notePart); }
            else if (tempStorage.Count < 1) { throw new NoteNotFoundException(notePart); }
            else { notes.Remove(tempStorage[0]); }
        }

        public DiscordEmbed PrintNotes(DiscordMember caller)
        {
            var footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "Refer to 'help note for more commands"
            };

            var embedBuilder = new DiscordEmbedBuilder()
            {
                Title = $"Notes by {caller.DisplayName}",
                Color = caller.Color,
                Footer = footer
            };

            foreach (Note note in notes)
            {
                if (note.AuthorID == caller.Id)
                {
                    embedBuilder.AddField($"{note.Content}", $"written {note.TimeStamp}");
                }
            }

            return embedBuilder;
        }

        public void Save()
        {
            try
            {
                FileStream writerFileStream = new FileStream(DATA_FILENAME, FileMode.Create, FileAccess.Write);
                this.formatter.Serialize(writerFileStream, notes);
                writerFileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to save! - {e}");
            }
        }

        public void Load()
        {
            if (File.Exists(DATA_FILENAME))
            {
                try
                {
                    FileStream readerFileStream = new FileStream(DATA_FILENAME, FileMode.Open, FileAccess.Read);
                    notes = (List<Note>)
                        formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Data file FOUND\nUnexpected reading problem\nException - - - {e}");
                }
            }
        }

        /// <summary>
        /// Checks to see if the mmax amount of notes has been reached
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="newNote"></param>
        /// <returns></returns>
        private bool IndexNotes(DiscordMember caller)
        {
            List<Note> tempStorage = new List<Note>();

            foreach (Note note in notes)
            {
                if (note.AuthorID == caller.Id)
                {
                    tempStorage.Add(note);
                }
            }

            if(tempStorage.Count < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [Serializable]
    public class Note
    {
        public string Content { get; private set; }
        public ulong AuthorID { get; private set; }
        public DateTimeOffset TimeStamp { get; private set; }

        public Note(string content, DiscordMember author, DateTimeOffset timestamp)
        {
            this.Content = content;
            this.AuthorID = author.Id;
            this.TimeStamp = timestamp;
        }
    }
}