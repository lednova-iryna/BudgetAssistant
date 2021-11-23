import { makeStyles } from "@mui/styles";

export const useTableStyles = makeStyles((theme) => ({
  table: { minWidth: 700 },
  tableContainer: {
    marginTop: "15px",
    borderRadius: 15,
    maxWidth: "100%",
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
