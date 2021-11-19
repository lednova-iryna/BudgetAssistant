import { Stack } from "@mui/material";
import IconButton from "@mui/material/IconButton";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import React from "react";
import { useTableStyles } from "../../../hooks/useTableStyle";

const PostButtons = () => {
    const classes = useTableStyles();
  return (
    <Stack  display='contents' direction="row" spacing={0} >
      <IconButton  aria-label="delete">
        <DeleteIcon />
      </IconButton>
      <IconButton  aria-label="edit">
        <EditIcon />
      </IconButton>
    </Stack>
  );
};

export default PostButtons;
