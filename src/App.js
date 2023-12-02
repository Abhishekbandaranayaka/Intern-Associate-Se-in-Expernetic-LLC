import React from 'react';
import './App.css';

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      notes: [],
      newNotes: '',
      selectedNoteId: null,
    };
  }

  API_URL = "http://localhost:5272/";

  componentDidMount() {
    this.refreshNotes();
  }

  refreshNotes() {
    fetch(this.API_URL + "api/todoapp/GetNotes")
      .then((response) => response.json())
      .then((data) => {
        this.setState({ notes: data });
      })
      .catch((error) => {
        console.error("Error fetching notes:", error);
      });
  }

  async addOrUpdateNotes() {
    const { newNotes, selectedNoteId } = this.state;
    const method = selectedNoteId ? "PUT" : "POST";
    const url = selectedNoteId ? `UpdateNotes?id=${selectedNoteId}` : "AddNotes";

    const data = new FormData();
    data.append(selectedNoteId ? "updatedNotes" : "newNotes", newNotes);

    fetch(this.API_URL + `api/todoapp/${url}`, {
      method: method,
      body: data,
    })
      .then((res) => res.json())
      .then((result) => {
        alert(result);
        this.setState({ newNotes: '', selectedNoteId: null });
        this.refreshNotes();
      })
      .catch((error) => {
        console.error(`Error ${method === "PUT" ? "updating" : "adding"} notes:`, error);
      });
  }

  async deleteNotes(id) {
    fetch(this.API_URL + `api/todoapp/DeleteNotes?id=${id}`, {
      method: "DELETE",
    })
      .then((res) => res.json())
      .then((result) => {
        alert(result);
        this.refreshNotes();
      })
      .catch((error) => {
        console.error("Error deleting notes:", error);
      });
  }

  async updateClick(id) {
    const { notes } = this.state;
    const selectedNote = notes.find((note) => note.id === id);

    if (selectedNote) {
      this.setState({ selectedNoteId: id, newNotes: selectedNote.description });
    }
  }

  render() {
    const { notes, newNotes } = this.state;
    return (
      <div className="App" style={{ fontFamily: 'Arial, sans-serif', padding: '20px' }}>
        <h2 style={{ color: '#333', textAlign: 'center' }}>To Do App</h2>
        <form style={{ marginBottom: '20px' }}>
          <input
            id="newNotes"
            value={newNotes}
            onChange={(e) => this.setState({ newNotes: e.target.value })}
            style={{ padding: '5px', marginRight: '10px' }}
          />
          <button
            type="button"
            onClick={() => this.addOrUpdateNotes()}
            style={{ background: '#4CAF50', color: 'white', padding: '8px 16px', cursor: 'pointer' }}>
            {this.state.selectedNoteId ? 'Update Notes' : 'Add Notes'}
          </button>
        </form>
        {notes.map((note) => (
          <div key={note.id} style={{ marginBottom: '10px', padding: '10px', background: '#f0f0f0' }}>
            <p>
              <b>{note.description}</b>
              <button
                type="button"
                onClick={() => this.updateClick(note.id)}
                style={{ marginLeft: '10px', background: '#2196F3', color: 'white', padding: '5px 10px', cursor: 'pointer' }}>
                Update Notes
              </button>
              <button
                type="button"
                onClick={() => this.deleteNotes(note.id)}
                style={{ marginLeft: '10px', background: '#f44336', color: 'white', padding: '5px 10px', cursor: 'pointer' }}>
                Delete Notes
              </button>
            </p>
          </div>
        ))}
      </div>
    );
  }
}

export default App;
