import React, { useState } from "react";
import PostForm from "./components/UI/post/PostForm";
import PostList from "./components/UI/post/PostList";
import "./App.css";
import { PostContext } from "./contexts/context";
import { MuiThemeProvider } from "material-ui/styles";

function App() {
  const [posts, setPost] = useState([]);
  return (
    <div className="App">
      <MuiThemeProvider>
        <PostContext.Provider value={{ posts, setPost }}>
          <PostForm></PostForm>
          <PostList></PostList>
        </PostContext.Provider>
      </MuiThemeProvider>
    </div>
  );
}

export default App;
