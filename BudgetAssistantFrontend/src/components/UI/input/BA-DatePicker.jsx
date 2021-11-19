import React from "react";
import TextField from "@mui/material/TextField";
import DatePicker from "@mui/lab/DatePicker";
import DateAdapter from "@mui/lab/AdapterMoment";
import LocalizationProvider from "@mui/lab/LocalizationProvider";

const BADatePicker = (props) => {
  return (
    <LocalizationProvider dateAdapter={DateAdapter}>
      <DatePicker
        disableFuture
        label="Date"
        {...props}
        renderInput={(params) => <TextField {...params} />}
      />
    </LocalizationProvider>
  );
};

export default BADatePicker;
