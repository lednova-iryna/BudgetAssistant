import React from "react";
import { Route, Routes } from "react-router-dom";
import "./App.css";
import MainAppBar from "./components/pages/MainAppBar";
import Posts from "./components/pages/Posts";
import Statistics from "./components/pages/Statistics";

function App() {
  return (
    <Routes>
      <Route path="/" element={<MainAppBar />}>
        <Route path="posts" element={<Posts />} />
        <Route path="statistics" element={<Statistics />} />
      </Route>
    </Routes>
  );
}

export default App;
