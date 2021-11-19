import React from "react";
import { TextField } from "@mui/material";

const BAInput = React.forwardRef((props) => {
  return <TextField variant="outlined" {...props} />;
});

export default BAInput;
