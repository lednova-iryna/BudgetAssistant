import Box from "@mui/material/Box";
import AppBar from "@mui/material/AppBar";
import React, { useState } from "react";
import { Outlet, Link } from "react-router-dom";
import Toolbar from "@mui/material/Toolbar";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import IconButton from "@mui/material/IconButton";
import MenuIcon from "@mui/icons-material/Menu";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Tooltip from "@mui/material/Tooltip";
import LoginControl from "../UI/loginControl/LoginControl";

const MainAppBar = () => {
  const [pageSelect, setPageSelect] = useState(null);
  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);

  const handleMenu = (event) => {
    setPageSelect(event.currentTarget);
  };
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      <LoginControl />
      <AppBar position="static" color="inherit">
        <Toolbar>
          <Tooltip title="Menu">
            <IconButton
              size="large"
              edge="start"
              color="inherit"
              aria-label="menu"
              sx={{ mr: 2 }}
              aria-expanded={open ? "true" : undefined}
              aria-haspopup="true"
              onClick={handleClick}
            >
              <MenuIcon />
            </IconButton>
          </Tooltip>
          <Menu
            id="menu-mainappbar"
            anchorEl={anchorEl}
            open={open}
            onClose={handleClose}
          >
            <MenuItem onClick={handleMenu}>
              <Link style={{textDecoration: "none"}} to="/posts">Posts</Link>
            </MenuItem>
            <MenuItem onClick={handleMenu}>
              <Link style={{textDecoration: "none"}} to="/statistics">Statistics</Link>
            </MenuItem>
          </Menu>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            There will be page name
          </Typography>
          {/* <Button color="inherit">Login</Button> */}
        </Toolbar>

        <div>
          <hr />
          <Outlet />
        </div>
      </AppBar>
    </Box>
  );
};

export default MainAppBar;
