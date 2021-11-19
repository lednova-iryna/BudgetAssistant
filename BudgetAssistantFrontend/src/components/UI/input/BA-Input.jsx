import React from "react";
import { TextField } from "@mui/material";

const BAInput = React.forwardRef((props, ref) => {
  return <TextField variant="outlined" {...props} />;
});

export default BAInput;
