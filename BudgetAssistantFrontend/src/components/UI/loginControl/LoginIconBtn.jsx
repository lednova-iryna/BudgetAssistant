import React, { useState } from "react";
import IconButton from "@mui/material/IconButton";
import AccountCircle from "@mui/icons-material/AccountCircle";
import Tooltip from "@mui/material/Tooltip";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import { Link } from "react-router-dom";

const LoginIconBtn = () => {
  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);

  const handleMenu = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClick = (event) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <div>
      <Tooltip title="My account">
        <IconButton
          size="large"
          aria-label="account of current user"
          aria-controls="menu-account"
          aria-haspopup="true"
          onClick={handleClick}
          color="inherit"
        >
          <AccountCircle />
        </IconButton>
      </Tooltip>
      <Menu
        id="menu-account-btn"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
      >
        <MenuItem onClick={handleMenu}>
          <Link style={{ textDecoration: "none" }} to="/client_profile">
            My profile
          </Link>
        </MenuItem>
        <MenuItem onClick={handleMenu}>
          <Link style={{ textDecoration: "none" }} to="/account_settings">
            Settings
          </Link>
        </MenuItem>
      </Menu>
    </div>
  );
};

export default LoginIconBtn;
