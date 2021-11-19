import { makeStyles } from "@mui/styles";

export const useTableStyles = makeStyles((theme) => ({
  table: { minWidth: 700 },
  tableContainer: {
    borderRadius: 15,
    margin: "10px 10px",
    maxWidth: 1000,
  },
  tableHeaderCell: {
    fontWeight: "bold",
    backgroundColor: "cadetblue",
    color: "white",
    textAlign: "center",
  },
  tableButtonCell: {
    textAlign: "right",
    width: "90px",
  },
}));
