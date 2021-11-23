import React, { useState } from "react";
import PostList from "../UI/post/PostList";
import "../../App.css";
import { PostContext } from "../../contexts/context";
import { MuiThemeProvider } from "material-ui/styles";
import PostForm from "../UI/post/PostForm";

const Posts = () => {
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
};

export default Posts;
