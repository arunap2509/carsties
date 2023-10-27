import { create } from "zustand";
import { createWithEqualityFn } from "zustand/traditional";

type State = {
    pageNumber: number;
    pageSize: number;
    pageCount: number;
    searchTerm: string;
    searchValue: string;
    orderBy: string;
    filterBy: string;
}

type Actions = {
    setParams: (params: Partial<State>) => void;
    reset: () => void;
    setSearchValue: (value: string) => void;
}

const initialState: State = {
    pageCount: 1,
    pageNumber: 1,
    pageSize: 12,
    searchTerm: '',
    searchValue: '',
    orderBy: 'make',
    filterBy: 'live'
}

export const useParamsStore = createWithEqualityFn<State & Actions>()((set) => ({
    ...initialState,
    setParams: (newParams: Partial<State>) => {
        set((state) => {
            if (newParams.pageNumber) {
                return { ...state, pageNumber: newParams.pageNumber };
            } else {
                return { ...state, ...newParams, pageNumber: 1 };
            }
        })
    },
    setSearchValue: (value) => set((state) => ({ ...state, searchValue: value })),
    reset: () => set(initialState)
}));